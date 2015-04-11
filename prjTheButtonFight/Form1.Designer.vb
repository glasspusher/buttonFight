<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.picField = New System.Windows.Forms.PictureBox()
        Me.lblPlayers = New System.Windows.Forms.Label()
        CType(Me.picField, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'picField
        '
        Me.picField.BackColor = System.Drawing.Color.White
        Me.picField.Location = New System.Drawing.Point(163, 21)
        Me.picField.Name = "picField"
        Me.picField.Size = New System.Drawing.Size(400, 400)
        Me.picField.TabIndex = 0
        Me.picField.TabStop = False
        '
        'lblPlayers
        '
        Me.lblPlayers.AutoSize = True
        Me.lblPlayers.Location = New System.Drawing.Point(3, 21)
        Me.lblPlayers.Name = "lblPlayers"
        Me.lblPlayers.Size = New System.Drawing.Size(53, 13)
        Me.lblPlayers.TabIndex = 1
        Me.lblPlayers.Text = "Players: 1"
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(575, 438)
        Me.Controls.Add(Me.lblPlayers)
        Me.Controls.Add(Me.picField)
        Me.DoubleBuffered = True
        Me.Name = "Form1"
        Me.Text = "Form1"
        CType(Me.picField, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents picField As System.Windows.Forms.PictureBox
    Friend WithEvents lblPlayers As System.Windows.Forms.Label

End Class
