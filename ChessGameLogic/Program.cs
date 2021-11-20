using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGameLogic
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {



                //Square s1 = null;
                //Square s2 = new Square(s1);
                //Square s3 = new Square(s2);
                //Console.WriteLine("s2 : " + s2);
                //Console.WriteLine("s3 : " + s3);
                //Console.WriteLine("s1 : " + s1);
                //Square square1 = new Square(new Piece(PieceTypes.BLACK_KING), ChessGameEntityColor.WHITE);
                ////Square square2 = new Square(new Piece(PieceTypes.WHITE_BISHOP), ChessGameEntityColor.BLACK);
                ////Square square3 = new Square(square1);
                // Square square4 = new Square(null);
                //Console.WriteLine("1 : " + square1);
                //Console.WriteLine("2 : " + square2);
                //Console.WriteLine("3 : " + square3);
                //Console.WriteLine("4 : " + square4);

                //Console.WriteLine("1 : " + square1);
                //square1.PieceStaiedOnASquareForTurn();
                //Console.WriteLine("1 : " + square1);

                //square1.PieceStaiedOnASquareForTurn();
                //Console.WriteLine("1 : " + square1);
                //square1.PieceMovedFromSquare();
                //Console.WriteLine("1 : " + square1);


                //Console.WriteLine(square1.ValidSquare);
                //Console.WriteLine(square4.ValidSquare);


            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + "\n" + e.StackTrace);
            }
            //List<Piece> pieces = new List<Piece>();
            //pieces.Add(new Piece(PieceTypes.BLACK_BISHOP));
            //pieces.Add(new Piece(PieceTypes.BLACK_KING));
            //pieces.Add(new Piece(PieceTypes.BLACK_KNIGHT));
            //pieces.Add(new Piece(PieceTypes.BLACK_POWN));
            //pieces.Add(new Piece(PieceTypes.BLACK_QUEEN));
            //pieces.Add(new Piece(PieceTypes.BLACK_ROOK));
            //pieces.Add(new Piece(null));
            //pieces.Add(new Piece(PieceTypes.WHITE_BISHOP));
            //pieces.Add(new Piece(PieceTypes.WHITE_KING));
            //pieces.Add(new Piece(PieceTypes.WHITE_KNIGHT));
            //pieces.Add(new Piece(PieceTypes.WHITE_POWN));
            //pieces.Add(new Piece(PieceTypes.WHITE_QUEEN));
            //pieces.Add(new Piece(PieceTypes.WHITE_ROOK));
            //pieces.Add(new Piece(null));

            ////foreach(Piece item in pieces.ToArray())
            ////{
            ////    Console.WriteLine(item);
            ////}

            //List<Piece> piecesCopies = new List<Piece>();
            //foreach (Piece item in pieces.ToArray())
            //{
            //    piecesCopies.Add(new Piece(item));
            //}

            ////for(int i=0, j=0; i<pieces.Count&&j<piecesCopies.Count; i++, j++)
            ////{
            ////    Console.WriteLine(pieces[i]);
            ////    Console.WriteLine("------------------------------------------------------------------------------------------");
            ////    Console.WriteLine(piecesCopies[j]);
            ////    Console.WriteLine("===========================================================================================");
            ////    Console.WriteLine("===========================================================================================");

            ////}

            //foreach (Piece item in pieces.ToArray())
            //{
            //    if (item.pieceHasSpecialFirstMoveIndicator())
            //    {
            //        item.PieceMovedForTheFirstTime();
            //    }
            //}
            ////foreach (Piece item in pieces.ToArray())
            ////{
            ////    Console.WriteLine(item);
            ////}
            //foreach (Piece item in pieces.ToArray())
            //{
            //    pieces.Remove(item);
            //}
            //pieces = null;

            //foreach (Piece item in piecesCopies.ToArray())
            //{
            //    //Console.WriteLine(item);
            //    Console.WriteLine(item.GetHashCode());
            //}

            //Console.WriteLine("{0} rival is {1}", ChessGameEntityColor.WHITE, ChessGameColorUtility.GetRivalColor(ChessGameEntityColor.WHITE));
            //Console.WriteLine("{0} rival is {1}", ChessGameEntityColor.BLACK, ChessGameColorUtility.GetRivalColor(ChessGameEntityColor.BLACK));



            //List<SquareCoordinate> listSqS = new List<SquareCoordinate>();
            ////listSqS.Add(new SquareCoordinate(1, 2));
            ////listSqS.Add(new SquareCoordinate(1, 2));
            ////listSqS.Add(new SquareCoordinate(2, 1));
            ////listSqS.Add(new SquareCoordinate(4, 0));
            ////listSqS.Add(new SquareCoordinate(-1, 3));
            ////listSqS.Add(new SquareCoordinate(5, -1));
            ////listSqS.Add(new SquareCoordinate(10, -10));
            ////listSqS.Add(new SquareCoordinate(2, 1));
            ////listSqS.Add(new SquareCoordinate(1, 2));

            //listSqS.Add(new SquareCoordinate(1, 'b'));
            //listSqS.Add(new SquareCoordinate(1, 'b'));
            //listSqS.Add(new SquareCoordinate(2, 'a'));
            //listSqS.Add(new SquareCoordinate(4, '@'));
            //listSqS.Add(new SquareCoordinate(-1, 'c'));
            //listSqS.Add(new SquareCoordinate(5, '^'));
            //listSqS.Add(new SquareCoordinate(10, 'm'));
            //listSqS.Add(new SquareCoordinate(2, 'a'));
            //listSqS.Add(new SquareCoordinate(1, 'b'));

            //Console.WriteLine("==============================================");
            //for (int i = 0; i < listSqS.Count; i++)
            //{
            //    Console.WriteLine(listSqS[i].GetHashCode());
            //}

            //foreach (SquareCoordinate item in listSqS.ToArray())
            //{
            //    Console.WriteLine(item);
            //    Console.WriteLine(item.ValidSquareCoordinates);
            //    Console.WriteLine("HashCode = " + item.GetHashCode());
            //    Console.WriteLine("==============================================");
            //}

            //Console.WriteLine("{0} {1} {2}", listSqS[0], listSqS[0].Equals(listSqS[1]) ? "=" : "!=", listSqS[1]);
            //Console.WriteLine("{0} {1} {2}", listSqS[1], listSqS[1].Equals(listSqS[2]) ? "=" : "!=", listSqS[2]);

            //Console.WriteLine("==============================================");

            //Console.WriteLine(listSqS[0].CompareTo(listSqS[0]));
            //Console.WriteLine(listSqS[0].CompareTo(listSqS[1]));
            //Console.WriteLine(listSqS[0].CompareTo(listSqS[2]));
            //Console.WriteLine(listSqS[2].CompareTo(listSqS[1]));

            //Console.WriteLine("==============================================");

            //List<SquareCoordinate> listSqCopies = new List<SquareCoordinate>();
            //foreach (SquareCoordinate item in listSqS.ToArray())
            //{
            //   // listSqCopies.Add(new SquareCoordinate(item));
            //    listSqCopies.Add(new SquareCoordinate(8,8));
            //    //listSqCopies.Add(new SquareCoordinate(1,1));
            //}
            //foreach (SquareCoordinate item in listSqCopies.ToArray())
            //{
            //    Console.WriteLine(item);
            //}

            //Console.WriteLine("==============================================");
            //foreach (SquareCoordinate item in listSqS.ToArray())
            //{
            //    Console.WriteLine(item);
            //}
            //Console.WriteLine("==============================================");
            //for(int i=0; i<listSqS.Count; i++)
            //{
            //    Console.WriteLine(listSqS[i].CompareTo(listSqCopies[i]));
            //}
            //Console.WriteLine("==============================================");
            //for (int i = 0; i < listSqS.Count; i++)
            //{
            //    Console.WriteLine(listSqS[i].Equals(listSqCopies[i]));
            //}


        }
    }
}
