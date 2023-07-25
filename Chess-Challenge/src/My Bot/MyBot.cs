using ChessChallenge.API;
using System.Collections.Generic;
using System;
using System.Linq;

public class MyBot : IChessBot
{   
    List<double> weight_embedding = new List<double> {0.9730796217918396,0.614592432975769,0.07421940565109253,0.029782414436340332,0.5213226079940796,0.8103286623954773,0.15611129999160767,0.4049001932144165,0.6014329791069031,0.48774802684783936,0.9331782460212708,0.005019545555114746,0.3061535954475403,0.6928005814552307,} ;
    List<double> weight_first_layer = new List<double> {0.07945466041564941,0.812091052532196,0.8718829154968262,0.3487513065338135,0.18254011869430542,0.9798083901405334,0.7202877998352051,0.9644587635993958,0.4720371961593628,0.8291995525360107,0.22545266151428223,0.11946803331375122,0.5906192064285278,0.5264113545417786,0.22610938549041748,0.7332460284233093,0.7167381048202515,0.4215547442436218,0.7737493515014648,0.2975529432296753,0.3800951838493347,0.7016407251358032,0.9638484716415405,0.4765649437904358,0.8809961080551147,0.09421157836914062,0.6606245040893555,0.5812562108039856,0.17074435949325562,0.4848129153251648,0.9687851071357727,0.27551811933517456,0.7275791168212891,0.4627816081047058,0.7956193089485168,0.9412174224853516,0.14281928539276123,0.09726530313491821,0.03268784284591675,0.5836650133132935,0.9838075637817383,0.8019949793815613,0.2831229567527771,0.826034665107727,0.07814759016036987,0.13502317667007446,0.36891603469848633,0.3108562231063843,0.8099081516265869,0.7218905091285706,0.6673977971076965,0.5407083630561829,0.39562928676605225,0.058968305587768555,0.6302012205123901,0.968704342842102,0.03448677062988281,0.3950764536857605,0.4660598039627075,0.49032002687454224,0.012421071529388428,0.9060983061790466,0.5691748261451721,0.5661485195159912,0.8022603392601013,0.6759422421455383,0.5385729670524597,0.41686034202575684,0.3953210711479187,0.9121642708778381,0.7020789384841919,0.3129118084907532,0.7508895397186279,0.022716760635375977,0.8860806822776794,0.27660369873046875,0.2852821946144104,0.03018277883529663,0.7687580585479736,0.7304773330688477,0.10550731420516968,0.9906513690948486,0.7812575101852417,0.03741908073425293,0.5671001076698303,0.1374727487564087,0.2230657935142517,0.30201101303100586,0.08623826503753662,0.1689104437828064,0.5918642282485962,0.5248088240623474,0.5657674074172974,0.11967915296554565,0.9088002443313599,0.5208162665367126,0.6496246457099915,0.008932650089263916,0.9905894994735718,0.8330756425857544,0.8060360550880432,0.8475689888000488,0.9741893410682678,0.19515645503997803,0.5358339548110962,0.04823434352874756,0.9772688150405884,0.9092148542404175,0.5392146110534668,0.6671314835548401,0.606061577796936,0.4689769744873047,0.6769447326660156,0.13500088453292847,0.7734261155128479,0.5613938570022583,0.5532103776931763,0.4589659571647644,0.6770647764205933,0.7902370691299438,0.1317920684814453,0.3239443302154541,0.2104550004005432,0.12050706148147583,0.21035736799240112,0.24735760688781738,0.5155957937240601,0.7307320237159729,} ;
    List<double> weight_second_layer = new List<double> {0.13095636665821075,0.2831968069076538,} ;
    // static bool GetBit(ulong value, int bitPosition)
    // {
    //     // Perform bitwise AND operation with a bitmask to extract the bit at the specified position
    //     ulong mask = (ulong)1 << bitPosition;
    //     ulong bitValue = value & mask;

    //     // Convert the extracted bit to a bool value
    //     bool result = bitValue != 0;

    //     return result;
    // }
    // public void set_value(List<bool> vec,ulong Bool_board,int ind_from){
    //     for(int i=ind_from;i<64+ind_from;i++){
    //         //System.Console.WriteLine("i:"+i);
    //         bool selected_bit = GetBit(Bool_board, i);
    //         vec[i]= selected_bit;
    //         //System.Console.WriteLine(vec[i]);
    //     }
    // }
    // public double board_evaluator_neural_network(Board board){
    //     List<string> rows = new List<string> {"a","b","c","d","e","f","g","h"};
    //     List<bool> vec_plateau = new List<bool>(new bool[768]);
    //     List<bool> white= new List<bool> {true,false};
    //     int cursor= 0;
    //     for(int c=0;c<2;c++){
    //         for(int p=0; p<6;p++){
    //             System.Console.WriteLine("p:"+p);
    //             var bool_chessboard_this_piece = board.GetPieceBitboard((PieceType)p,white[c]);
    //             set_value(vec_plateau,bool_chessboard_this_piece,cursor);
    //             cursor+=64;
    //         }
    //     }
    //     string string_weight = "azertyazertyazertyazertyazertyazertyazertyazertyazertyazertyazertyazertyazertyazertyazertyazertyazertyazertyazertyazertyazertyazertyazertyazertyazertyazertyazertyazertyazertyazertyazertyazertyazertyazertyazertyazertyazertyazertyazkdzx";
    //     var nb_cols = 2;
    //     List<double> result_vec = new List<double>(new double[nb_cols]);
    //     for(int num_cols_weight_matrix=0;num_cols_weight_matrix<nb_cols;num_cols_weight_matrix++){
    //         for(int i=0;i<768;i++){
    //             var ind_in_string = nb_cols*768+i;
    //             var extracted_weight = (double)string_weight[ind_in_string];
    //             result_vec[num_cols_weight_matrix]=Convert.ToDouble(vec_plateau[i])*extracted_weight;
    //             System.Console.WriteLine(result_vec[num_cols_weight_matrix]);
    //         }
    //     }
    //     // var vec1 = new List<double> { 1.0, 3.0, 3.0, 4.0, 9.0 };
    //     // var vec2 = new List<double> { 1.0, 3.0, 3.0, 4.0, 9.0 }; // 14
    //     // var vec3 = new List<double> { 1.0}; //10
    //     // var vec4 = new List<double> {}; //9
    //     // string test = "azerty"; // 1 token per letter
    //     return 0.1;

    // }

    public static float Sigmoid(double value) {
        return 1.0f / (1.0f + (float) Math.Exp(-value));
    }

    public double board_evaluator_neural_network_V2(Board board){
        //List<double> new_test_weight_first_layer =  new List<double> { 1.0, 3.0, 3.0, 4.0, 9.0, 23.5 };
        //string string_weight_first_layer = "egkds";
        List<string> rows = new List<string> {"a","b","c","d","e","f","g","h"};
        List<double> result_vec_embedding = new List<double>(new double[64]);
        for(int i=0;i<8;i++){
            for(int j=0;j<8;j++){
                var piece = board.GetPiece(new Square(rows[i]+(j+1).ToString()));
                var ind_in_string = (int)piece.PieceType; 
                if(piece.IsWhite){
                    ind_in_string+=7;
                }
                result_vec_embedding[i*8+j]=Sigmoid(weight_embedding[ind_in_string]);
            }
        }
        List<double> result_first_layer = new List<double>(new double[2]);
        for(int i=0;i<64;i++){
            result_first_layer[0]+=Sigmoid(weight_first_layer[i]*result_vec_embedding[i]);
            result_first_layer[1]+=Sigmoid(weight_first_layer[i+64]*result_vec_embedding[i]);
        }
        double result_second_layer =0.0;
        for(int i=0;i<2;i++){
             result_second_layer+=2.0*Sigmoid(weight_second_layer[i]*result_first_layer[i])-1.0;
        }

        return result_second_layer;
    }

    public int get_best_move_index(List<double> c_val, List<int> nb_exploration, int nb_sim_done){
        var nb_possible_moves = c_val.Count;
        double exploration_factor = 1.0*Math.Sqrt(2); //0.1
        var win_proba = new List<double>(new double[nb_possible_moves]);
        for (int i = 0; i < nb_possible_moves; i++){
            win_proba[i] = (c_val[i]/nb_exploration[i])+(exploration_factor*((double)Math.Sqrt(nb_sim_done)/(double)nb_exploration[i]));
        }
        double maxValue = win_proba.Max();

        int maxIndex = win_proba.ToList().IndexOf(maxValue);
        return maxIndex;
    }

    // public double board_evaluator(Board board){
    //     //None = 0, Pawn = 1, Knight = 2, Bishop = 3, Rook = 4, Queen = 5, King = 6
    //     var piece_value = new List<double> { 1.0, 3.0, 3.0, 4.0, 9.0 };
    //     var board_value_white = 0.0; 
    //     var board_value_black = 0.0; 
    //     for (int i = 0; i < 5; i++){
    //         board_value_white += board.GetPieceList((PieceType)i+1,true).Count*piece_value[i];
    //         board_value_black += board.GetPieceList((PieceType)i+1,false).Count*piece_value[i];
    //     }
    //     if(board_value_white+board_value_black==0){
    //         return 0.0;
    //     }
    //     return 1.0*((board_value_white)/(board_value_white+board_value_black)); //0.1, with 1.0 this mean that the probability of wining is the fraction of piece value of our color
    //     // Be carefull with to big value this can lead to prefer wining some pieces more than checkmates
    // }
    public Move Get_move_for_simulation(Board board){
        Random rng = new();
        Move[] moves = board.GetLegalMoves(true);
        if(moves.Length == 0){
            moves = board.GetLegalMoves(false);
        }
        // Pick a random move to play if nothing better is found
        Move moveToPlay = moves[rng.Next(moves.Length)];
        return moveToPlay;
    }

    // public Move Get_move_for_simulation_V2(Board board){
    //     bool AmIWhite = board.IsWhiteToMove;
    //     Move[] moves = board.GetLegalMoves(false);
    //     double best_score = -2.0;
    //     Move best_move = moves[0];
    //     for(int i=0;i<moves.Length;i++){
    //         board.MakeMove(moves[i]);
    //         var score_this_move = board_evaluator_neural_network_V2(board);
    //         if(!AmIWhite){ // Not sure about this
    //             score_this_move = -score_this_move;
    //         }
    //         if(score_this_move> best_score){
    //             best_score = score_this_move;
    //             best_move = moves[i];
    //         }
    //         board.UndoMove(moves[i]);
    //     }
    //     return best_move;
    //}

    public double Simulate(Board board){
        bool AmIWhite = board.IsWhiteToMove;
        var max_depth = 200; 
        var depth = 0;
        //Random rng = new();
        while(!board.IsInCheckmate() && depth<max_depth &&  board.GetLegalMoves().Length!=0){
            var moveToPlay = Get_move_for_simulation(board);
            board.MakeMove(moveToPlay);
            depth+=1;
        }
        //System.Console.WriteLine(board.GetFenString());
        //System.Console.WriteLine(board.GetLegalMoves());
        if(!board.IsInCheckmate()){
            double board_value = board_evaluator_neural_network_V2(board);
            if(AmIWhite){
                return board_value;
            }
            else{
                return -board_value;
            }
           
        }
        else{
            if((board.IsWhiteToMove && AmIWhite) || (!board.IsWhiteToMove && !AmIWhite)){
                return -1.0;
            }
            // else if(board.IsWhiteToMove && !AmIWhite){
            //     return 1.0;
            // }
            // else if(!board.IsWhiteToMove && AmIWhite){
            //     return 1.0;
            // }
            else{ // 
                return 1.0;
            }
        }

    }

    public Move Think(Board board, Timer timer)
    {
        //neural_network(board);
        Move[] moves = board.GetLegalMoves();
        //int nb_possible_moves = moves.Length;
        var c_val = new List<double>(new double[moves.Length]);
        var nb_exploration = new List<int>(new int[moves.Length]);
        //var nb_simmulations = 1500; //1500
        //Random rng = new();
        string origin_fen = board.GetFenString();
        for (int nb_done_sim = 0; nb_done_sim < 500; nb_done_sim++){ // Define the number of simulations
            //var chosen_move = rng.Next(moves.Length);
            int chosen_move = -1;
            if(nb_done_sim<moves.Length){ // at least one time of each
                chosen_move = nb_done_sim;
            }
            else{
                chosen_move = get_best_move_index(c_val, nb_exploration, nb_done_sim);
            }
            //System.Console.WriteLine("chosen_move:"+ chosen_move);
            Move moveToPlay = moves[chosen_move];

            Board new_board = Board.CreateBoardFromFEN(origin_fen);
            new_board.MakeMove(moveToPlay);
            c_val[chosen_move]+= Simulate(new_board);
            nb_exploration[chosen_move]+=1;
        }
        //System.Console.WriteLine("------------------");
        var win_proba = new List<float>(new float[moves.Length]);
        for (int i = 0; i < moves.Length; i++){
            win_proba[i] = (float)c_val[i]/(float)(nb_exploration[i]+1);
            System.Console.WriteLine(win_proba[i]);
        }
        float maxValue = win_proba.Max();
        System.Console.WriteLine("max value:"+maxValue);
        int maxIndex = win_proba.ToList().IndexOf(maxValue);

        return moves[maxIndex];
    }
}
