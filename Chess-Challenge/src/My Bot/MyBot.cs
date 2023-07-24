using ChessChallenge.API;
using System.Collections.Generic;
using System;
using System.Linq;

public class MyBot : IChessBot
{   
    static bool GetBit(ulong value, int bitPosition)
    {
        // Perform bitwise AND operation with a bitmask to extract the bit at the specified position
        ulong mask = (ulong)1 << bitPosition;
        ulong bitValue = value & mask;

        // Convert the extracted bit to a bool value
        bool result = bitValue != 0;

        return result;
    }
    public void set_value(List<bool> vec,ulong Bool_board,int ind_from){
        for(int i=ind_from;i<64+ind_from;i++){
            System.Console.WriteLine("i:"+i);
            bool selected_bit = GetBit(Bool_board, i);
            vec[i]= selected_bit;
            System.Console.WriteLine(vec[i]);
        }
    }
    public double neural_network(Board board){
        List<string> rows = new List<string> {"a","b","c","d","e","f","g","h"};
        List<bool> vec_plateau = new List<bool>(new bool[768]);
        List<bool> white= new List<bool> {true,false};
        int cursor= 0;
        for(int c=0;c<2;c++){
            for(int p=0; p<6;p++){
                System.Console.WriteLine("p:"+p);
                var bool_chessboard_this_piece = board.GetPieceBitboard((PieceType)p,white[c]);
                set_value(vec_plateau,bool_chessboard_this_piece,cursor);
                cursor+=64;
            }
        }
        string string_weight = "azertyazertyazertyazertyazertyazertyazertyazertyazertyazertyazertyazertyazertyazertyazertyazertyazertyazertyazertyazertyazertyazertyazertyazertyazertyazertyazertyazertyazertyazertyazertyazertyazertyazertyazertyazertyazertyazertyazertyazertyazertyazertyazertyazertyazertyazertyazertyazertyazertyazertyazertyazertyazertyazertyazertyazertyazerttyazertyazertyazertyazertyazertyazertyazertyazertyazerttyazertyazertyazertyazertyazertya";
        var nb_cols = 2;
        List<double> result_vec = new List<double>(new double[nb_cols]);
        for(int num_cols_weight_matrix=0;num_cols_weight_matrix<nb_cols;num_cols_weight_matrix++){
            for(int i=0;i<768;i++){
                var ind_in_string = nb_cols*768+i;
                var extracted_weight = (double)string_weight[ind_in_string];
                result_vec[num_cols_weight_matrix]=Convert.ToDouble(vec_plateau[i])*extracted_weight;
                System.Console.WriteLine(result_vec[num_cols_weight_matrix]);
            }
        }
        // var vec1 = new List<double> { 1.0, 3.0, 3.0, 4.0, 9.0 };
        // var vec2 = new List<double> { 1.0, 3.0, 3.0, 4.0, 9.0 }; // 14
        // var vec3 = new List<double> { 1.0}; //10
        // var vec4 = new List<double> {}; //9
        // string test = "azerty"; // 1 token per letter
        return 0.1;

    }
    public int get_best_move_index(List<double> c_val, List<int> nb_exploration, int nb_sim_done){
        var nb_possible_moves = c_val.Count;
        double exploration_factor = 0.1*Math.Sqrt(2); //0.1
        var win_proba = new List<double>(new double[nb_possible_moves]);
        for (int i = 0; i < nb_possible_moves; i++){
            win_proba[i] = (c_val[i]/nb_exploration[i])+(exploration_factor*((double)Math.Sqrt(nb_sim_done)/(double)nb_exploration[i]));
        }
        double maxValue = win_proba.Max();

        int maxIndex = win_proba.ToList().IndexOf(maxValue);
        return maxIndex;
    }

    public double board_evaluator(Board board){
        //None = 0, Pawn = 1, Knight = 2, Bishop = 3, Rook = 4, Queen = 5, King = 6
        var piece_value = new List<double> { 1.0, 3.0, 3.0, 4.0, 9.0 };
        var board_value_white = 0.0; 
        var board_value_black = 0.0; 
        for (int i = 0; i < 5; i++){
            board_value_white += board.GetPieceList((PieceType)i+1,true).Count*piece_value[i];
            board_value_black += board.GetPieceList((PieceType)i+1,false).Count*piece_value[i];
        }
        if(board_value_white+board_value_black==0){
            return 0.0;
        }
        return 1.0*((board_value_white)/(board_value_white+board_value_black)); //0.1, with 1.0 this mean that the probability of wining is the fraction of piece value of our color
        // Be carefull with to big value this can lead to prefer wining some pieces more than checkmates
    }
    public double Simulate(Board board,bool AmIWhite){
        var max_depth = 200; 
        var depth = 0;
        Random rng = new();
        while(!board.IsInCheckmate() && depth<max_depth){
            Move[] moves = board.GetLegalMoves(true);
            if(moves.Length == 0){
                moves = board.GetLegalMoves(false);
                if(moves.Length == 0){
                    break;
                }
            }
            // Pick a random move to play if nothing better is found
            Move moveToPlay = moves[rng.Next(moves.Length)];
            board.MakeMove(moveToPlay);
            depth+=1;
        }
        //System.Console.WriteLine(board.GetFenString());
        //System.Console.WriteLine(board.GetLegalMoves());
        if(!board.IsInCheckmate()){
            double board_value = board_evaluator(board);
            if(AmIWhite){
                return board_value;
            }
            else{
                return 0.5-board_value;
            }
           
        }
        else{
            if(board.IsWhiteToMove && AmIWhite){
                return -1.0;
            }
            else if(board.IsWhiteToMove && !AmIWhite){
                return 1.0;
            }
            else if(!board.IsWhiteToMove && AmIWhite){
                return 1.0;
            }
            else{ // !board.IsWhiteToMove && !AmIWhite
                return -1.0;
            }
        }

    }

    public Move Think(Board board, Timer timer)
    {
        //neural_network(board);
        bool AmIWhite = board.IsWhiteToMove;
        Move[] moves = board.GetLegalMoves();
        int nb_possible_moves = moves.Length;
        var c_val = new List<double>(new double[nb_possible_moves]);
        var nb_exploration = new List<int>(new int[nb_possible_moves]);
        var nb_simmulations = 1500; //1500
        Random rng = new();
        string origin_fen = board.GetFenString();
        for (int nb_done_sim = 0; nb_done_sim < nb_simmulations; nb_done_sim++){
            //var chosen_move = rng.Next(moves.Length);
            int chosen_move = -1;
            if(nb_done_sim<nb_possible_moves){ // at least one time of each
                chosen_move = nb_done_sim;
            }
            else{
                chosen_move = get_best_move_index(c_val, nb_exploration, nb_done_sim);
            }
            //System.Console.WriteLine("chosen_move:"+ chosen_move);
            Move moveToPlay = moves[chosen_move];

            Board new_board = Board.CreateBoardFromFEN(origin_fen);
            new_board.MakeMove(moveToPlay);
            var value = Simulate(new_board,AmIWhite);
            c_val[chosen_move]+=value;
            nb_exploration[chosen_move]+=1;
        }
        System.Console.WriteLine("------------------");
        var win_proba = new List<float>(new float[nb_possible_moves]);
        for (int i = 0; i < nb_possible_moves; i++){
            win_proba[i] = (float)c_val[i]/(float)(nb_exploration[i]+1);
            //System.Console.WriteLine(win_proba[i]);
        }
        float maxValue = win_proba.Max();
        System.Console.WriteLine("max value:"+maxValue);
        int maxIndex = win_proba.ToList().IndexOf(maxValue);

        return moves[maxIndex];
    }
}
