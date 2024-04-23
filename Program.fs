open type Tensorflow.KerasApi
open Tensorflow
open Tensorflow.NumPy
open type Tensorflow.Keras.Utils.np_utils
open type Tensorflow.Binding
open System.Collections.Generic
open System
open Plotly.NET
open Plotly.NET.ImageExport

let layers = keras.layers

let cnn =
    let input = keras.Input(shape = Shape(28, 28, 1))

    let modelFlow =
        input
        |> layers.Conv2D(16, 5, activation = "relu").Apply
        |> layers.MaxPooling2D(2).Apply
        |> layers.Conv2D(36, 5, activation = "relu").Apply
        |> layers.MaxPooling2D(2).Apply
        |> layers.Flatten().Apply
        |> layers.Dense(128, activation = "relu").Apply

    let output = modelFlow |> layers.Dense(10, activation = "softmax").Apply

    keras.Model(input, output, "CNN")

let prepareData () =
    let (x_train, y_train, x_test, y_test) =
        keras.datasets.mnist.load_data().Deconstruct()

    // reshape
    let x_train = (x_train.reshape (Shape(-1, 28, 28, 1))).astype (TF_DataType.TF_FLOAT)
    let x_test = (x_test.reshape (Shape(-1, 28, 28, 1))).astype (TF_DataType.TF_FLOAT)

    // normalize
    let x_train = (x_train / 255).numpy ()
    let x_test = (x_test / 255).numpy ()

    // one-hot encoding
    let y_train = to_categorical (y_train, 10)
    let y_test = to_categorical (y_test, 10)

    (x_train, y_train, x_test, y_test)

let showTrainHistory (trainHistory: Dictionary<string, List<float32>>) =    
    let train =
        Chart.Line(x = [ 0 .. trainHistory["accuracy"].Count ], y = trainHistory["accuracy"], Name = "train")

    let validation =
        Chart.Line(x = [ 0 .. trainHistory["val_accuracy"].Count ], y = trainHistory["val_accuracy"], Name = "test")

    let chart=Chart.combine ([| train; validation |]) |> Chart.withTitle "Train History"

    chart|>Chart.saveJPG("trainHistory.png")
    
    chart|>Chart.show

let train (model: Tensorflow.Keras.Engine.IModel) (x_train: NDArray, y_train) (x_test,y_test) =
    // train model by feeding data and labels.
    model.fit (x_train, y_train, batch_size = 200, epochs = 20, validation_split = 0.2f, verbose = 1)
    |> fun callback -> callback.history
    |> showTrainHistory

    model.evaluate (x_test, y_test, verbose = 2)
    |> Seq.iter (fun record -> printfn $"{record.Key} = {record.Value}")

    model.save ("cnn")
    

[<EntryPoint>]
let main argv =
    let (x_train, y_train, x_test, y_test) = prepareData ()
    printfn $"{x_train.GetType()} {x_test.GetType()}"
    printfn "%s" (cnn.GetType().ToString())
    cnn.summary ()

    cnn.compile (
        optimizer = keras.optimizers.Adam(1e-3f),
        loss = keras.losses.CategoricalCrossentropy(),
        metrics = [| "acc" |]
    )

    train cnn (x_train, y_train) (x_test,y_test)

    0
