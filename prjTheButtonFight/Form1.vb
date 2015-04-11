Imports System.Net.WebSockets
Imports System.Threading
Imports System.Text.RegularExpressions
Imports System.Text
Imports Newtonsoft.Json.Linq

Public Class Form1
    Private wsServer As Uri
    Private secs, x, y As Integer
    Private oldSecsLeft As Integer
    Private hasTicked As Boolean = False
    Private hasBeenClicked As Boolean = False
    Private fieldBMP As Bitmap
    Private y1List As New List(Of Integer)
    Private y1Idx As Integer
    Private y2List As New List(Of Integer)
    Private y2Idx As Integer
    Private rnd As New Random()
    Private g As Graphics
    Private b As New SolidBrush(Color.White)
    Private players1 As Integer = 1
    Private players2 As Integer = 0
    Private players As Integer = players1 + players2
    Private multi As Integer = 40
    Private fnt As New Font("Courier", 20)
    Private strBr As New SolidBrush(Color.Gray)
    Private gameReady As Boolean = False
    Private side As Integer = 2



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
            y1List.Add(i)
            y2List.Add(i)
        Next

        y1Idx = rnd.Next(0, y1List.Count)
        y = y1List(y1Idx) * multi
        y1List.RemoveAt(y1Idx)

        Debug.WriteLine("first y:" & y.ToString & "Adding player 1, side 1")


    End Sub


    Private Async Sub start()

        Dim clr As Color
        Using ws As New ClientWebSocket()
            Dim result As WebSocketReceiveResult

            Await ws.ConnectAsync(wsServer, CancellationToken.None)

            While True
                Dim bytesReceived As New ArraySegment(Of Byte)(New Byte(1023) {})
                result = Await ws.ReceiveAsync(bytesReceived, CancellationToken.None)
                Dim s As String = Encoding.UTF8.GetString(bytesReceived.Array)
                Dim json As JObject = JObject.Parse(s)



                secs = CInt(json.SelectToken("payload").SelectToken("seconds_left"))

                If hasTicked AndAlso secs >= oldSecsLeft Then

                    If players + 1 > (picField.Height / multi) * 2 Then
                        Exit While
                    End If

                    If side = 1 Then
                        players1 += 1
                        Debug.WriteLine("button has been clicked! new y:" & y.ToString & "   Adding player: " & players1.ToString & ", side: " & side.ToString)
                        y1Idx = rnd.Next(0, y1List.Count - 1)
                        x = 0
                        y = y1List(y1Idx) * multi
                        y1List.RemoveAt(y1Idx)
                        side = 2
                    Else
                        players2 += 1
                        Debug.WriteLine("button has been clicked! new y:" & y.ToString & "   Adding player: " & players2.ToString & ", side: " & side.ToString)
                        y2Idx = rnd.Next(0, y2List.Count - 1)
                        x = picField.Width - multi - 1
                        y = y2List(y2Idx) * multi
                        y2List.RemoveAt(y2Idx)
                        side = 1
                    End If

                    players = players1 + players2

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

                g.FillRectangle(b, x, y, multi, multi)
                g.DrawString(secs.ToString, fnt, strBr, New PointF(x, y + 3))

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
