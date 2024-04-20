# FSCNN

採用 F# 來實現一非常簡單的 CNN (convolutional neural network，卷積神經網路) ，以 mnist 資料集訓練手寫辨識，使用[TensorFlow.NET](https://github.com/SciSharp/TensorFlow.NET)

目前只能實現訓練，儲存的模型經測試在 python 端 tensorflow 可用，不知為何在 F# 就不可用，實在是非常可惜

會弄這東西主要是探究拿 dotnet 平台搓AI、ML之類的東西，現階段的可用性還是不及 python

社群這東西實在是現實的恐怖，主流實現之外的平台幾乎處於放生的程度。

如果頭鐵堅持不用主流實現的話可能要處處自幹，用愛發電，偏偏我又很菜，想用愛發電也發不成XD

在研究的過程中發現微軟官方好像有出一個基於 [TensorFlow.NET](https://github.com/SciSharp/TensorFlow.NET) 的東西，叫做 [ML.NET](https://github.com/dotnet/machinelearning)，初步看來好像跟 Azure 有不小的關係？再看再研究
