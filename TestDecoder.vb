Imports Viterbi

Module TestDecoder

    Sub Main()

        Dim polys As Integer() = {15, 17}
        Dim enc As New Viterbi.Encoder(polys)
        Dim decod As New Viterbi.Decoder(polys)
        Console.WriteLine($"Encoder, decoder {enc.ToString()}")

        Dim message As Boolean() = {True, False, True, False}
        Dim encoded As IEnumerable(Of Boolean) = enc.Encode(message)
        Dim restored As IEnumerable(Of Boolean) = decod.Decode(encoded)

        Console.WriteLine($"Message: {message.ToBinString()}".Insert(0, vbNewLine))
        Console.WriteLine($"Encoded: {encoded.ToBinString()}")
        If restored.BinaryEquals(message) Then
            Console.ForegroundColor = ConsoleColor.Green
        Else
            Console.ForegroundColor = ConsoleColor.Red
        End If
        Console.WriteLine($"Restord: {restored.ToBinString()}")
        Console.ForegroundColor = ConsoleColor.Gray

        message = {False, True, False, False, True, False, False, False, False, True, False, False, False, True, False, True}
        encoded = enc.Encode(message)
        restored = decod.Decode(encoded)

        Console.WriteLine($"Message: {message.ToBinString()}".Insert(0, vbNewLine))
        Console.WriteLine($"Encoded: {encoded.ToBinString()}")
        If restored.BinaryEquals(message) Then
            Console.ForegroundColor = ConsoleColor.Green
        Else
            Console.ForegroundColor = ConsoleColor.Red
        End If
        Console.WriteLine($"Restord: {restored.ToBinString()}")
        Console.ForegroundColor = ConsoleColor.Gray
        
        Dim message2 As String = "Hello, Я Soltau.ru! :)"
        Console.WriteLine($"Message: {message2}".Insert(0, vbNewLine))
        encoded = enc.Encode(message2)

        For percent As Integer = 0 To 9
            Console.ForegroundColor = ConsoleColor.Yellow
            Console.WriteLine($"Errors: {percent}%".Insert(0, vbNewLine))
            Console.ForegroundColor = ConsoleColor.Gray
            Dim errEncoded = GetMessageWithErrors(encoded, percent)
            restored = decod.Decode(errEncoded)
            Console.WriteLine($"Message: {message2.ToBinString()}")
            Console.WriteLine($"Encoded: {ToBinString(errEncoded)}")
            Console.WriteLine($"Restord: {restored.ToBinString()}")
            If (restored.ToUtf8String() = message2) Then
                Console.ForegroundColor = ConsoleColor.Green
            Else
                Console.ForegroundColor = ConsoleColor.Red
            End If
            Console.WriteLine($"Restord: {restored.ToUtf8String()}")
            Console.ForegroundColor = ConsoleColor.Gray
        Next

        'Декодируем файл:
        Dim src As String = "c:\temp\encoded.txt"
        Dim dst As String = "c:\temp\restored.bin"
        Dim sw As New Stopwatch()
        sw.Start()
        Console.WriteLine($"{Now} Decodeing started {src}.".Insert(0, vbNewLine))
        Using srcFs As New IO.FileStream(src, IO.FileMode.Open), destFs As New IO.FileStream(dst, IO.FileMode.Create)
            Using r As New IO.StreamReader(srcFs), w As New IO.BinaryWriter(destFs)
                decod.Decode(r, w)
            End Using
        End Using
        sw.Stop()
        Console.WriteLine($"{Now} Decoding finished {dst} ({sw.Elapsed.TotalSeconds:F1} sec)")

        Console.ReadLine()
    End Sub




End Module
