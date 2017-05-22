using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BukaCongklak
{
    class Program
    {
        

        static void Main(string[] args)
        {
            Game game = new Game();
            game.initGame();//ronde 1
            game.congklakBoard.drawBoard();

            //to test ronde 2
            //SkipRonde2(game);
            //end to test ronde 2

            //jalankan selama tidak mati jalan
            while (!game.isSomeoneMatiJalan()){
                Console.WriteLine();
                Console.Write("Giliran " + game.currentPlayer.PlayerName + " , silahkan masukkan nomor rumah yang anda ingin jalankan... :    ");
                string inputTurnStr = Console.ReadLine();
                if (isValidPlayerInput(inputTurnStr,game)) {

                    game.currentPlayer.Jalan(game,Convert.ToInt32(inputTurnStr),true);
                }
            }
            Console.WriteLine("---------------------------------------------------------");
            Console.WriteLine(game.menang.PlayerName + " memenangkan ronde ke-" + game.ronde + ". Initialisasi Ronde Berikutnya");
            Console.WriteLine("---------------------------------------------------------");
            Console.ReadKey();
            //logika game berikutnya.
             game.ronde = game.ronde + 1;
            //initialisasi ronde 2+++:
            //sisi player 1
            var storeHousePlayer1 = game.players[0].getOwnStoreHouseNodeFromBoard(game.congklakBoard);
            var currentHouse = storeHousePlayer1.PreviousOrLast();
            //kumpulin sisa sisa dari rumah 1 -7
            for(var i = 0; i<game.amountofHousesForEachPlayer;i++) {
                storeHousePlayer1.Value.qtyBiji = storeHousePlayer1.Value.qtyBiji + currentHouse.Value.qtyBiji;
                currentHouse.Value.qtyBiji = 0;
                currentHouse = currentHouse.PreviousOrLast();
            }
            game.congklakBoard.drawBoard();
            //
            Console.ReadKey();
            //Bagi bagi Player 1
            currentHouse = storeHousePlayer1.PreviousOrLast();
            for (var i = 0; i < game.amountofHousesForEachPlayer; i++)
            {
                int qtyBagiBiji = Math.Min(storeHousePlayer1.Value.qtyBiji, game.qtyofInitialBijiEachHouse); // 7 atau sisa
                storeHousePlayer1.Value.qtyBiji = storeHousePlayer1.Value.qtyBiji - qtyBagiBiji;
                currentHouse.Value.qtyBiji = currentHouse.Value.qtyBiji + qtyBagiBiji;
                //jika kurang dari 7 = ngacang
                if (currentHouse.Value.qtyBiji < game.qtyofInitialBijiEachHouse)
                {
                    currentHouse.Value.setNgacang(true);
                }
                else {
                    currentHouse.Value.setNgacang(false);
                }
                currentHouse = currentHouse.PreviousOrLast();
            }
            game.congklakBoard.drawBoard();
            //
            Console.ReadKey();

            //sisi player 2
            var storeHousePlayer2 = game.players[1].getOwnStoreHouseNodeFromBoard(game.congklakBoard);
            currentHouse = storeHousePlayer2.PreviousOrLast();
            //kumpulin sisa sisa dari rumah 8 - 14
            for (var i = 0; i < game.amountofHousesForEachPlayer; i++)
            {
                storeHousePlayer2.Value.qtyBiji = storeHousePlayer2.Value.qtyBiji + currentHouse.Value.qtyBiji;
                currentHouse.Value.qtyBiji = 0;
                currentHouse = currentHouse.PreviousOrLast();
            }
            game.congklakBoard.drawBoard();
            //
            Console.ReadKey();
            currentHouse = storeHousePlayer2.PreviousOrLast();
            for (var i = 0; i < game.amountofHousesForEachPlayer; i++)
            {
                int qtyBagiBiji = Math.Min(storeHousePlayer2.Value.qtyBiji, game.qtyofInitialBijiEachHouse); // 7 atau sisa
                storeHousePlayer2.Value.qtyBiji = storeHousePlayer2.Value.qtyBiji - qtyBagiBiji;
                currentHouse.Value.qtyBiji = currentHouse.Value.qtyBiji + qtyBagiBiji;

                //jika kurang dari 7 = ngacang
                if (currentHouse.Value.qtyBiji < game.qtyofInitialBijiEachHouse)                {
                    currentHouse.Value.setNgacang(true);
                }
                else{
                    currentHouse.Value.setNgacang(false);
                }
                currentHouse = currentHouse.PreviousOrLast();
            }
            game.congklakBoard.drawBoard();
            //
            Console.ReadKey();

            //jalankan selama tidak mati jalan
            while (!game.isSomeoneMatiJalan())
            {
                Console.WriteLine();
                Console.Write("Giliran " + game.currentPlayer.PlayerName + " , silahkan masukkan nomor rumah yang anda ingin jalankan... :    ");
                string inputTurnStr = Console.ReadLine();
                if (isValidPlayerInput(inputTurnStr, game))
                {

                    game.currentPlayer.Jalan(game, Convert.ToInt32(inputTurnStr), true);
                }
            }

        }

        static private void SkipRonde2(Game game)
        {
            //int jumlahBiji = game.qtyofInitialBijiEachHouse * game.amountofHousesForEachPlayer * game.players.Length;

            //set cara kasar
            game.congklakBoard.linkedHouses.ElementAt(0).qtyBiji = 1; //1 
            game.congklakBoard.linkedHouses.ElementAt(1).qtyBiji = 3; //2 
            game.congklakBoard.linkedHouses.ElementAt(2).qtyBiji = 5; //3 
            game.congklakBoard.linkedHouses.ElementAt(3).qtyBiji = 0; //4 
            game.congklakBoard.linkedHouses.ElementAt(4).qtyBiji = 1; //5 
            game.congklakBoard.linkedHouses.ElementAt(5).qtyBiji = 1; //6 
            game.congklakBoard.linkedHouses.ElementAt(6).qtyBiji = 0; //7 
            game.congklakBoard.linkedHouses.ElementAt(7).qtyBiji = 58; //Storehouse Player 1 
            game.congklakBoard.linkedHouses.ElementAt(8).qtyBiji = 0; //8
            game.congklakBoard.linkedHouses.ElementAt(9).qtyBiji = 0; //9 
            game.congklakBoard.linkedHouses.ElementAt(10).qtyBiji = 0; //10 
            game.congklakBoard.linkedHouses.ElementAt(11).qtyBiji = 0; //11 
            game.congklakBoard.linkedHouses.ElementAt(12).qtyBiji = 0; //12
            game.congklakBoard.linkedHouses.ElementAt(13).qtyBiji = 0; //13
            game.congklakBoard.linkedHouses.ElementAt(14).qtyBiji = 0; //14
            game.congklakBoard.linkedHouses.ElementAt(15).qtyBiji = 29; //Storehouse Player 2
            game.menang = game.players[0];

            game.congklakBoard.drawBoard();

        }

        static bool isValidPlayerInput(string inputTurnStr,Game game) {
            int parsedInt = 0;
            if (int.TryParse(inputTurnStr, out parsedInt)){
                if (parsedInt > 0 && parsedInt <= game.qtyofInitialBijiEachHouse * 2){
                    return true;
                }
                else{
                    Console.WriteLine("Input tidak valid. Nomor rumah yang tersedia hanya dari 1 - " + game.qtyofInitialBijiEachHouse*2);
                    return false;
                }
            }
            else{
                Console.WriteLine("Silahkan masukan angka yang valid dan benar.");
                return false;
            }
        }
    }
}
