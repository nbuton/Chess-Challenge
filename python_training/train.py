import pandas as pd
import numpy as np
import chess
import torch
from tqdm import tqdm

from model import chess_board_evaluator

def convert_fen_to_input_nn(fen_notation):
    # Vocab order None = 0, Pawn = 1, Knight = 2, Bishop = 3, Rook = 4, Queen = 5, King = 6
    dico_vocab_pieces = {"NONE": 0, "P" : 1, "N" : 2, "B" : 3, "R" : 4, "Q" : 5, "K" : 6}
    vocab_rows = ["a","b","c","d","e","f","g","h"]
    board = chess.Board(fen=fen_notation)
    input_list = np.zeros((64,14),dtype=np.float32)
    for i in range(8):
        for j in range(8):
            square_name = vocab_rows[i]+str(j+1)
            piece = board.piece_at(chess.parse_square(square_name))
            piece = str(piece)
            ind = dico_vocab_pieces[piece.upper()]
            if piece.isupper(): # White piece
                ind+=7
            input_list[i*8+j][ind]=1.0
    return torch.tensor(input_list)


# Data from https://www.kaggle.com/datasets/ronakbadhe/chess-evaluations?resource=download&select=chessData.csv
df_chessboard = pd.read_csv("python_training/chessData.csv")
df_chessboard = df_chessboard.sample(frac=0.01)
df_chessboard["Evaluation"] = df_chessboard["Evaluation"].apply(lambda x: x.replace("#",""))
df_chessboard["Evaluation"] = df_chessboard["Evaluation"].apply(lambda x: x.replace("\ufeff",""))
print(df_chessboard)

df_chessboard = df_chessboard.astype({'Evaluation': 'int32'})
print(df_chessboard)
print(df_chessboard.describe())
df_chessboard["Evaluation"] = (df_chessboard["Evaluation"]-df_chessboard["Evaluation"].min())/(df_chessboard["Evaluation"].max()-df_chessboard["Evaluation"].min())


model = chess_board_evaluator()
criterion = torch.nn.MSELoss()
optimizer = torch.optim.Adam(model.parameters(),lr=1e-5)
batch_size = 1024

log_step = 10000

all_loss = []
nb_steps = 0
for index, row in tqdm(df_chessboard.iterrows(),total=len(df_chessboard)):
    fen_notation = row["FEN"]
    stockfish_score = torch.tensor(row["Evaluation"])
    input = convert_fen_to_input_nn(fen_notation)
    output = model(input)
    loss = criterion(output, stockfish_score)
    all_loss.append(loss.item())
    loss.backward()
    if nb_steps%log_step==0:
        print("loss:",np.mean(all_loss))
        all_loss = []
    if nb_steps%batch_size==0:
        optimizer.step()
        optimizer.zero_grad()
    nb_steps+=1

path_save_model = "chess_evaluator.pth"
torch.save(model, path_save_model)