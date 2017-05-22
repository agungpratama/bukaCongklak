using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BukaCongklak
{
    public class Game
    {
        public Player[] players { get; set; }
        public CongklakBoard congklakBoard { get; set; }

        public bool isAlreadyMuter = false;

        private int amountofPlayers = 2;
        public int amountofHousesForEachPlayer = 7;
        public int qtyofInitialBijiEachHouse = 7;
        public Player menang;
        public int ronde;

        public Player currentPlayer;

        protected void initPlayers()
        {
            //init players
            this.players = new Player[amountofPlayers];
            for (var i = 0; i < amountofPlayers; i++)
            {
                players[i] = new Player()
                {
                    PlayerName = "Player" + (i + 1)
                };
            }
        }

        protected void initCongklakBoard() {
            this.congklakBoard = new CongklakBoard();
            this.congklakBoard.init(this.players, amountofHousesForEachPlayer, qtyofInitialBijiEachHouse);


        }



        public void initGame()
        {
            ronde = 1;
            initPlayers();
            initCongklakBoard();
            currentPlayer = this.players[0];
        }

        public void changePlayerTurn() {
            isAlreadyMuter = false;
            if (currentPlayer == this.players[0])
            {
                currentPlayer = this.players[1];
            }
            else {
                currentPlayer = this.players[0];
            }
        }

        public bool isSomeoneMatiJalan() {
            //check masing masing player apakah mati jalan atau tidak;
            //cek di seluruh rumah yang ada, jika semua 0 ya udh abis.
            int jumlahBijiTemp = 0;
            //cek player 1 ( index 0 s/d < banyak rumah ) default: 7 => index[0] - index[6]
            for (var i = 0; i < this.amountofHousesForEachPlayer; i++) {
                jumlahBijiTemp  = jumlahBijiTemp  + this.congklakBoard.linkedHouses.ElementAt(i).qtyBiji;
            }
            if(jumlahBijiTemp == 0) {
                Console.WriteLine("Semua rumah yang disisi Player1 = kosong, Player1 Kalah jalan");
                this.menang = this.players[1];//player2 menang;
                return true; //kalah player1
            }
            jumlahBijiTemp = 0;
            //cek player 2 ( index dari banyak rumah + 1  s/d  < jumlah banyak rumah * 2) default:7 => index[8] - index [13] 
            for (var j = amountofHousesForEachPlayer+1; j < this.amountofHousesForEachPlayer*2;j++ ) {
                jumlahBijiTemp = jumlahBijiTemp + this.congklakBoard.linkedHouses.ElementAt(j).qtyBiji;
            }
            if (jumlahBijiTemp == 0) {
                Console.WriteLine("Semua rumah yang disisi Player2 = kosong, Player2 Kalah jalan");
                this.menang = this.players[0];//player 1 menang;
                return true; //kalah player2
            }
            return false;
        }
    }
}
