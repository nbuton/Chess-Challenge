import torch

def convert_weight_to_string(pre_str, weights):
    string_weights=pre_str
    for w in weights:
        string_weights += str(w.item())+","
    string_weights += "} ;"
    print(string_weights)
model_path = "chess_evaluator.pth"
model = torch.load(model_path)

convert_weight_to_string("List<double> weight_embedding = new List<double> {",model.weight_embedding.flatten())
convert_weight_to_string("List<double> weight_first_layer = new List<double> {",model.weight_first_layer.flatten())
convert_weight_to_string("List<double> weight_second_layer = new List<double> {",model.weight_second_layer.flatten())