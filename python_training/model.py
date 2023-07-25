import torch

class chess_board_evaluator(torch.nn.Module):
    def __init__(self):
        super().__init__()
        self.weight_embedding = torch.nn.Parameter(torch.zeros((14,1)))
        self.weight_first_layer = torch.nn.Parameter(torch.zeros((64,2)))
        self.weight_second_layer = torch.nn.Parameter(torch.zeros((2,1)))
        print("self.weight_embedding:",self.weight_embedding.shape)
        print("self.weight_first_layer:",self.weight_first_layer.shape)
        print("self.weight_second_layer:", self.weight_second_layer.shape)
    

        torch.nn.init.uniform_(self.weight_embedding)
        torch.nn.init.uniform_(self.weight_first_layer)
        torch.nn.init.uniform_(self.weight_second_layer)

    def forward(self,input):
        '''
        Input of shape (64,14)
        '''
        result_embedding = torch.sigmoid(torch.matmul(input,self.weight_embedding))
        result_first_layer = torch.sigmoid(torch.matmul(result_embedding.T,self.weight_first_layer))
        result_second_layer = 2.0*torch.sigmoid(torch.matmul(result_first_layer,self.weight_second_layer))-1.0
        return result_second_layer