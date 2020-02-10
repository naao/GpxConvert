'v.0.20 2020-2-10 JogNoteのワークアウト総カロリーを各LAP距離で分割し反映
'v.0.13 2020-2-8 ラップデータが1つ以下の場合の不具合を修正
'v.0.12 2020-2-7 エラー処理を追加
'v.0.1  2020-2-4 初版

Imports System.IO
Imports System.Runtime.Serialization.Json
Imports System.Text
Imports System.Xml

Public Class FrmGpxConvert
    '変換用テンプレートファイルの場所・ファイル名を変更した場合は以下を編集
    Friend strTemplateFileName As String = ".\template.tcx"
    '読込みJSON／GPX／書出しTCXフォルダ名（ファイル群の場所）を変更した場合は以下を編集
    Friend strJsonFolderName As String = ".\json.\"
    Friend strGpxFolderName As String = ".\gpx.\"
    Friend strTcxFolderName As String = ".\tcx_w.\"

    Private Sub Execute()

        'jsonフォルダ内の、.jsonファイル一覧を取得
        Dim jsonFiles() = Directory.GetFiles(strJsonFolderName, "*.json", SearchOption.TopDirectoryOnly)
        If jsonFiles.Count < 1 Then
            Exit Sub 'jsonファイルが無い場合は何もしない
        End If

        TbxMessage.Text = Nothing

        'jsonフォルダ内のjsonファイル群を1つずつ処理
        For Each jsonFile As String In jsonFiles

            'jsonファイルを読み込み、noteRootクラスのjsonRootオブジェクトに格納
            Dim oJsonRoot As noteRoot = ReadJson(Of noteRoot)(jsonFile)

            Dim strNoteDate As String = oJsonRoot.note.[date]
            Dim strNoteDiary As String = oJsonRoot.note.diary

            Dim strWorkoutMeter As String
            Dim strWorkoutSec As String
            Dim strWorkoutKcal As String
            Dim strGpxFilename As String

            TbxMessage.Text += "[日付] " + strNoteDate + vbCrLf
            TbxMessage.Text += "[日記] " + strNoteDiary + vbCrLf

            'リストBOXへファイル名を追記
            LbxJsonFiles.Items.Add(jsonFile)

            Dim workoutList() = oJsonRoot.note.workouts
            If workoutList.Count < 1 Then
                Exit For    'ワークアウトが無い場合は無視
            End If

            'テンプレート ファイルを読込み、TCXファイルの出力用DOMオブジェクト
            Dim domOut As XmlDocument
            Dim elmActivity As XmlElement
            Dim ndlLaps As XmlNodeList
            Try
                'ファイルを開く
                domOut = ReadDom(strTemplateFileName)
                elmActivity = domOut.GetElementsByTagName("Activity")(0)
                ndlLaps = elmActivity.GetElementsByTagName("Lap")
            Catch ex As System.IO.FileNotFoundException
                MessageBox.Show("テンプレートファイルが見つかりません", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit For
            End Try

            For Each workouts As workoutList In workoutList

                strWorkoutMeter = workouts.meter
                strWorkoutSec = workouts.sec
                strWorkoutKcal = workouts.kcal
                strGpxFilename = strGpxFolderName + workouts.route

                Dim lapsList() As lapsList = workouts.laps
                'GPX XMLファイルをDOMオブジェクトに読込む
                Dim domGpx As XmlDocument
                Dim ndlTrkPoints As XmlNodeList

                Try
                    'ファイルを開く
                    domGpx = ReadDom(strGpxFilename)
                    ndlTrkPoints = domGpx.GetElementsByTagName("trkpt")
                Catch ex As System.IO.FileNotFoundException
                    MessageBox.Show("紐づけられたGPXファイルが見つかりません" + strGpxFilename, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit For
                Catch ex As System.IO.IOException
                    'IOExceptionをキャッチした時
                    MessageBox.Show("ファイルがロックされている可能性があります" + strGpxFilename, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit For
                End Try

                'リストBOXへファイル名を追記
                LbxGpxFiles.Items.Add(strGpxFilename)

                '開始時刻
                Dim elmWorkoutStartTime As XmlElement = ndlTrkPoints(0)
                Dim dtWorkoutStartTime As DateTime = System.TimeZoneInfo.ConvertTimeToUtc(elmWorkoutStartTime.GetElementsByTagName("time")(0).InnerText)

                '経過時間
                Dim iElapsedTime As Int32 = 0
                Dim iLapNum As Int32 = 0
                Dim iLapDistance As Int32
                Dim iLapTime As Int32
                Dim nmLapSpeed As Double = 0.0

                Dim iTrNumPoints As Int32 = 0

                'LAPデータ
                Dim elmNewLap As XmlElement
                Dim elmNewTracks As XmlElement
                Dim elmNewTrackPoint As XmlElement

                Dim strStartTime As String

                '***** 各GPXポイントを処理 *****
                For Each elmTrkPoint As XmlElement In ndlTrkPoints
                    Dim strLat As String = elmTrkPoint.GetAttribute("lat")
                    Dim strLon As String = elmTrkPoint.GetAttribute("lon")
                    Dim dtGpsPoint As DateTime = System.TimeZoneInfo.ConvertTimeToUtc(elmTrkPoint.GetElementsByTagName("time")(0).InnerText)

                    Dim tsElapsedTime As New TimeSpan(0, 0, 0, iElapsedTime)

                    Try
                        '***** LAP区切りの処理 *****
                        If ((dtWorkoutStartTime + tsElapsedTime) <= dtGpsPoint) And (iLapNum <= lapsList.Count) Then
                            'LAP開始時間
                            Dim dtLapStartTime As DateTime = dtGpsPoint
                            Dim strLapStartTime As String

                            '***** LAP区間時間・距離の計算 *****
                            If iLapNum < lapsList.Count Then
                                '経過時間にLAP時間を加算
                                iElapsedTime += CInt(lapsList(iLapNum).sec)

                                '最終ラップを超えるまでは区間距離（今回ラップ距離-前回ラップ距離）
                                If 0 < iLapNum Then
                                    iLapDistance = lapsList(iLapNum).meter - lapsList(iLapNum - 1).meter
                                Else
                                    iLapDistance = lapsList(iLapNum).meter
                                End If
                                '時間はラップ時間のまま代入
                                iLapTime = lapsList(iLapNum).sec

                            ElseIf iLapNum = lapsList.Count Then '最終ラップ分が記録されていないので、最終ラップデータから計算する
                                If 0 < iLapNum Then
                                    '最終ラップを超えたら、総走行距離から引く
                                    iLapDistance = CInt(strWorkoutMeter) - lapsList(iLapNum - 1).meter
                                Else
                                    iLapDistance = CInt(strWorkoutMeter)
                                End If
                                '時間はトータル時間から、経過時間　を差し引く
                                iLapTime = CInt(strWorkoutSec) - iElapsedTime
                            End If

                            nmLapSpeed = iLapDistance / iLapTime

                            'DOMにLAPを追加
                            elmNewLap = ndlLaps(0).CloneNode(True)
                            elmActivity.AppendChild(elmNewLap)

                            strLapStartTime = dtLapStartTime.ToString("yyyy-MM-dd\THH:mm:ss\.000Z")
                            elmNewLap.SetAttribute("StartTime", strLapStartTime)
                            If iLapNum < 1 Then
                                strStartTime = strLapStartTime
                            End If

                            elmNewLap.GetElementsByTagName("DistanceMeters")(0).InnerText = iLapDistance
                            elmNewLap.GetElementsByTagName("TotalTimeSeconds")(0).InnerText = iLapTime
                            elmNewLap.GetElementsByTagName("Calories")(0).InnerText = CInt(CInt(strWorkoutKcal) * iLapDistance / CInt(strWorkoutMeter))
                            elmNewLap.GetElementsByTagName("ns3:AvgSpeed")(0).InnerText = nmLapSpeed

                            'LAPに追加するGPSデータツリー
                            elmNewTracks = elmNewLap.GetElementsByTagName("Track")(0)

                            TbxMessage.Text += "[LAP-" + (iLapNum + 1).ToString + "] " + iLapDistance.ToString + "m/" + iLapTime.ToString + "s" + vbCrLf

                            iLapNum += 1

                            iTrNumPoints = 0

                        End If

                    Catch ex As System.Exception
                        'すべての例外をキャッチする
                        '例外の説明を表示する
                        MessageBox.Show(ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Exit For

                    End Try

                    elmNewTrackPoint = elmNewTracks.GetElementsByTagName("Trackpoint")(0).CloneNode(True)
                    elmNewTrackPoint.GetElementsByTagName("Time")(0).InnerText = dtGpsPoint.ToString("yyyy-MM-dd\THH:mm:ss\.000Z")
                    elmNewTrackPoint.GetElementsByTagName("LatitudeDegrees")(0).InnerText = strLat
                    elmNewTrackPoint.GetElementsByTagName("LongitudeDegrees")(0).InnerText = strLon

                    elmNewTracks.AppendChild(elmNewTrackPoint)

                    If iTrNumPoints < 1 Then
                        elmNewTracks.RemoveChild(elmNewTracks.GetElementsByTagName("Trackpoint")(0))
                    End If

                    iTrNumPoints += 1

                Next    'For Each elmTrkPoint

                elmActivity.RemoveChild(elmActivity.GetElementsByTagName("Lap")(0))
                elmActivity.GetElementsByTagName("Id")(0).InnerText = strStartTime

                Dim strTcxFileName As String = strTcxFolderName + workouts.route.Replace(".gpx", ".tcx")

                'DOMをファイルに出力
                domOut.Save(strTcxFileName)

                'リストBOXへファイル名を追記
                LbxTcxFiles.Items.Add(strTcxFileName)

            Next    'For Each workouts
        Next    'For Each jsonFile
    End Sub

    Private Function ReadDom(file) As XmlDocument

        ' GPX XMLファイルを読込むDOMオブジェクト
        Dim dom As XmlDocument
        ' XMLテンプレートドキュメントを初期化
        dom = New XmlDocument
        ' XMLテンプレートファイルを読み込み
        dom.Load(file)

        Return dom

    End Function


    'jsonファイルを読み込み、指定オブジェクトクラスに返す関数
    Private Function ReadJson(Of T)(ByVal jsonFile As String) As T

        Dim result As T
        Dim streamJson As String = File.ReadAllText(jsonFile, Encoding.UTF8)
        Dim serializer As New DataContractJsonSerializer(GetType(noteRoot))

        Using stream As New IO.MemoryStream(Encoding.UTF8.GetBytes(streamJson))
            result = DirectCast(serializer.ReadObject(stream), T)
        End Using

        Return result

    End Function

    Private Sub FrmGpxConvert_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub BtnExec_Click(sender As Object, e As EventArgs) Handles BtnExec.Click
        Execute()
    End Sub

End Class

Public Class noteRoot
    Public Property note As noteList
End Class

Public Class noteList
    Public Property note_id As String
    Public Property [date] As String
    Public Property weather As String
    Public Property location As String
    Public Property diary As String
    Public Property physical_conditions As String
    Public Property comments As String()
    Public Property workouts As workoutList()
End Class

Public Class workoutList
    Public Property workout_id As String
    Public Property name As String
    Public Property sec As String
    Public Property meter As String
    Public Property kcal As String
    Public Property steps As String
    Public Property memo As String
    Public Property laps As lapsList()
    Public Property route As String
End Class

Public Class lapsList
    Public Property sec As String
    Public Property meter As String
End Class
