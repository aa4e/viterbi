# viterbi

Project realizes Viterbi `encoder` and `decoder`.

## Usage

### Encode message:

```vbnet
Dim polys As Integer() = {15, 17}
Dim encoder As New Viterbi.Encoder(polys)
Dim message As String = "Hello, Viterbi!"
Dim encodedMessage As Boolean() = encoder.Encode(message)
```

### Decode message:

```vbnet
Dim decoder As New Viterbi.Decoder(polys)
Dim restored As Boolean() = decoder.Decode(encodedMessage)
```
