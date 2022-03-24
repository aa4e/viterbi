Imports Viterbi

Module TestEncoder

    Sub Main()
        Console.WriteLine("OCT -> DEC".Insert(0, vbNewLine))
        Dim octs As Integer() = {15, 3574, 67, 17, 5, 7, 353}
        For Each oct As Integer In octs
            Dim dec = oct.ToDecimal()
            Console.WriteLine($"{oct} = {dec}")
        Next

        Console.WriteLine("DEC -> OCT".Insert(0, vbNewLine))
        Dim decs As Integer() = {13, 116, 15, 5, 7, 235}
        For Each dec As Integer In decs
            Dim oct = dec.ToOctal()
            Console.WriteLine($"{dec} = {oct}")
        Next

        Console.WriteLine("DEC -> BIN".Insert(0, vbNewLine))
        For Each dec As Integer In decs
            Dim bin = dec.ToBinary()
            Console.WriteLine($"{dec} = {bin}")
        Next

        Dim coder As New Viterbi.Encoder({15, 17})
        Console.WriteLine($"Encoder {coder.ToString()}".Insert(0, vbNewLine))

        Console.WriteLine($"Register length: {coder.RegisterLength}".Insert(0, vbNewLine))
        Console.WriteLine("Register states:".Insert(0, vbNewLine))
        For Each state In coder.CoderStates
            Console.WriteLine($"{state.Key}{vbTab}{Convert.ToString(state.Key, 2).PadLeft(coder.RegisterLength, "0"c)}{vbTab}{Convert.ToString(state.Value, 2).PadLeft(coder.Polynoms.Length, "0"c)}")
        Next

        Console.WriteLine("Transitions grid:".Insert(0, vbNewLine))
        For Each t In coder.Transitions
            For Each tr As Transition In t
                Console.WriteLine(tr.ToString())
            Next
        Next

        Threading.ThreadPool.QueueUserWorkItem(Sub()
                                                   Dim sw As New Stopwatch()
                                                   sw.Start()
                                                   Dim src As String = "c:\temp\test.txt"
                                                   Dim dst As String = "c:\temp\encoded.txt"
                                                   Console.WriteLine($"Encoding started {src}")
                                                   Using srcFs As New IO.FileStream(src, IO.FileMode.Open), destFs As New IO.FileStream(dst, IO.FileMode.Create)
                                                       Using br As New IO.BinaryReader(srcFs), wr As New IO.StreamWriter(destFs, Text.Encoding.ASCII)
                                                           coder.Encode(br, wr)
                                                       End Using
                                                   End Using
                                                   Console.WriteLine($"Saved {dst}, taken {sw.Elapsed.TotalSeconds:F1} sec")
                                               End Sub)
        Dim encMessage2 As IEnumerable(Of Boolean) = coder.Encode({True, False, True, False})
        Console.WriteLine($"Encoded 1010: {ToBinString(encMessage2)}".Insert(0, vbNewLine))

        Dim encMessage1 As IEnumerable(Of Boolean) = coder.Encode({True, True, True, False,
                                                          False, True, False, True,
                                                          False, True, True, True,
                                                          True, True, False, False,
                                                          False, True, True, True,
                                                          False, True, False, False,
                                                          False, False, False, True,
                                                          False, False, False, False,
                                                          True, True, False, False,
                                                          False, False, True, True,
                                                          True, True, True, True, False}) '1110_0101_0111_1100_0111_0100_0001_0000_1100_0011_1111_0

        Console.WriteLine($"Message: 111001010111110001110100000100001100001111110".Insert(0, vbNewLine))
        Console.WriteLine($"Encod:{vbTab}{ToBinString(encMessage1)}")
        Dim checkStr2 As String = "110001011000111000101101101001101111000101010001110000111101110011001010110011000110101001"
        Console.WriteLine($"Check:{vbTab}{checkStr2}")
        Dim resCheck2 As Boolean = String.Equals(ToBinString(encMessage1), checkStr2)
        If resCheck2 Then
            Console.ForegroundColor = ConsoleColor.Green
        Else
            Console.ForegroundColor = ConsoleColor.Red
        End If
        Console.WriteLine($"Match: {resCheck2}")
        Console.ForegroundColor = ConsoleColor.Gray

        Console.ReadLine()
    End Sub

End Module
