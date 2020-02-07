<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmGpxConvert
    Inherits System.Windows.Forms.Form

    'フォームがコンポーネントの一覧をクリーンアップするために dispose をオーバーライドします。
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Windows フォーム デザイナーで必要です。
    Private components As System.ComponentModel.IContainer

    'メモ: 以下のプロシージャは Windows フォーム デザイナーで必要です。
    'Windows フォーム デザイナーを使用して変更できます。  
    'コード エディターを使って変更しないでください。
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.LbxGpxFiles = New System.Windows.Forms.ListBox()
        Me.LbxJsonFiles = New System.Windows.Forms.ListBox()
        Me.LbxTcxFiles = New System.Windows.Forms.ListBox()
        Me.TbxMessage = New System.Windows.Forms.TextBox()
        Me.BtnExec = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'LbxGpxFiles
        '
        Me.LbxGpxFiles.FormattingEnabled = True
        Me.LbxGpxFiles.ItemHeight = 12
        Me.LbxGpxFiles.Location = New System.Drawing.Point(30, 215)
        Me.LbxGpxFiles.Name = "LbxGpxFiles"
        Me.LbxGpxFiles.Size = New System.Drawing.Size(182, 124)
        Me.LbxGpxFiles.TabIndex = 0
        '
        'LbxJsonFiles
        '
        Me.LbxJsonFiles.FormattingEnabled = True
        Me.LbxJsonFiles.ItemHeight = 12
        Me.LbxJsonFiles.Location = New System.Drawing.Point(30, 62)
        Me.LbxJsonFiles.Name = "LbxJsonFiles"
        Me.LbxJsonFiles.Size = New System.Drawing.Size(182, 124)
        Me.LbxJsonFiles.TabIndex = 1
        '
        'LbxTcxFiles
        '
        Me.LbxTcxFiles.FormattingEnabled = True
        Me.LbxTcxFiles.ItemHeight = 12
        Me.LbxTcxFiles.Location = New System.Drawing.Point(30, 361)
        Me.LbxTcxFiles.Name = "LbxTcxFiles"
        Me.LbxTcxFiles.Size = New System.Drawing.Size(182, 124)
        Me.LbxTcxFiles.TabIndex = 2
        '
        'TbxMessage
        '
        Me.TbxMessage.Location = New System.Drawing.Point(258, 182)
        Me.TbxMessage.Multiline = True
        Me.TbxMessage.Name = "TbxMessage"
        Me.TbxMessage.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.TbxMessage.Size = New System.Drawing.Size(261, 303)
        Me.TbxMessage.TabIndex = 3
        '
        'BtnExec
        '
        Me.BtnExec.Location = New System.Drawing.Point(298, 48)
        Me.BtnExec.Name = "BtnExec"
        Me.BtnExec.Size = New System.Drawing.Size(158, 80)
        Me.BtnExec.TabIndex = 4
        Me.BtnExec.Text = "変換実行"
        Me.BtnExec.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 45)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(30, 12)
        Me.Label1.TabIndex = 5
        Me.Label1.Text = "Json"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(12, 200)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(27, 12)
        Me.Label2.TabIndex = 6
        Me.Label2.Text = "GPX"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(15, 346)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(51, 12)
        Me.Label3.TabIndex = 7
        Me.Label3.Text = "TCX出力"
        '
        'FrmGpxConvert
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(591, 504)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.BtnExec)
        Me.Controls.Add(Me.TbxMessage)
        Me.Controls.Add(Me.LbxTcxFiles)
        Me.Controls.Add(Me.LbxJsonFiles)
        Me.Controls.Add(Me.LbxGpxFiles)
        Me.Name = "FrmGpxConvert"
        Me.Text = "GpxConvert"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents LbxGpxFiles As ListBox
    Friend WithEvents LbxJsonFiles As ListBox
    Friend WithEvents LbxTcxFiles As ListBox
    Friend WithEvents TbxMessage As TextBox
    Friend WithEvents BtnExec As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
End Class
