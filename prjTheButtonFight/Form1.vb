Imports System.Net.WebSockets
Imports System.Threading
Imports System.Text.RegularExpressions
Imports System.Text
Imports Newtonsoft.Json.Linq

Public Class Form1
    Private wsServer As Uri
    Private secs As Integer
    Private oldSecsLeft As Integer
    Private hasTicked As Boolean = False
    Private hasBeenClicked As Boolean = False
    Private fieldBMP As Bitmap
    Private yList As New List(Of Integer)
    Private yIdx, y As Integer
    Private rnd As New Random()
    Private g As Graphics
    Private b As New SolidBrush(Color.White)
    Private players As Integer = 1
    Private multi As Integer = 40
    Private fnt As New Font("Courier", 20)
    Private strBr As New SolidBrush(Color.Gray)


    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim request As System.Net.HttpWebRequest = System.Net.HttpWebRequest.Create("http://www.reddit.com/r/thebutton")
        Dim response As System.Net.HttpWebResponse = request.GetResponse()
        Dim sr As System.IO.StreamReader = New System.IO.StreamReader(response.GetResponseStream())
        Dim sourcecode As String = sr.ReadToEnd()
        Dim pattern As String = "wss:\/\/(.+?)"""
        Dim m As Match = Regex.Match(sourcecode, pattern)

        fieldBMP = New Bitmap(picField.Width, picField.Height)
        g = Graphics.FromImage(fieldBMP)
        'g.CompositingMode = Drawing2D.CompositingMode.SourceCopy
        'g.CompositingQuality = Drawing2D.CompositingQuality.HighQuality
        'g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
        'g.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
        'g.TextRenderingHint = Drawing.Text.TextRenderingHint.ClearTypeGridFit

        wsServer = New Uri(m.Value.Trim(""""))

        For i As Integer = 0 To (picField.Height / multi) - 1
            yList.Add(i)
        Next

        yIdx = rnd.Next(0, yList.Count)
        y = yList(yIdx) * multi
        yList.RemoveAt(yIdx)

        Debug.WriteLine("first y:" & y.ToString)


    End Sub


    Private Async Sub start()

        Dim clr As Color
        Using ws As New ClientWebSocket()
            Dim result As WebSocketReceiveResult

            Await ws.ConnectAsync(wsServer, CancellationToken.None)

            While yList.Count <> 0
                Dim bytesReceived As New ArraySegment(Of Byte)(New Byte(1023) {})
                result = Await ws.ReceiveAsync(bytesReceived, CancellationToken.None)
                Dim s As String = Encoding.UTF8.GetString(bytesReceived.Array)
                Dim json As JObject = JObject.Parse(s)



                secs = CInt(json.SelectToken("payload").SelectToken("seconds_left"))

                If hasTicked AndAlso secs >= oldSecsLeft Then
                    yIdx = rnd.Next(0, yList.Count - 1)
                    y = yList(yIdx) * multi
                    yList.RemoveAt(yIdx)

                    Debug.WriteLine("button has been clicked! new y:" & y.ToString)
                    players += 1

                    lblPlayers.Text = "Players: " & players.ToString
                Else

                End If

                hasTicked = True
                oldSecsLeft = secs

                'Debug.WriteLine(json.SelectToken("payload").SelectToken("seconds_left"))
                Select Case secs
                    Case Is >= 52
                        clr = Color.Purple
                    Case Is >= 42
                        clr = Color.Blue
                    Case Is >= 32
                        clr = Color.Green
                    Case Is >= 22
                        clr = Color.Yellow
                    Case Is >= 12
                        clr = Color.Orange
                    Case Is >= 1
                        clr = Color.Red
                    Case 0
                        clr = Color.White
                End Select

                b.Color = clr

                g.FillRectangle(b, 0, y, multi, multi)
                g.DrawString(secs.ToString, fnt, strBr, New PointF(0, y + 3))

                picField.Image = fieldBMP
                picField.Refresh()
            End While
        End Using

        Debug.WriteLine("game ready!!!")
    End Sub

    Private Sub Form1_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        start()
    End Sub
End Class
