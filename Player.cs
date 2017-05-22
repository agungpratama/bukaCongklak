using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BukaCongklak
{
    public class Player
    {
        public string PlayerName { get; set; }

        public LinkedListNode<House>  getOwnStoreHouseNodeFromBoard(CongklakBoard board) {
            var firstHouse = board.linkedHouses.Find(board.linkedHouses.First());
            LinkedListNode<House> currentHouse = firstHouse;
            while (!(currentHouse.Value.isAStoreHouse() == true && currentHouse.Value.BelongToPlayer == this)) {
                currentHouse = currentHouse.NextOrFirst();
            }
            return currentHouse;
        }

        public void Jalan(Game game, int nomorRumah,bool isInitialJalan)
        {
            var board = game.congklakBoard;
            //translate inputan nomor rumah ke IndexElementAt di linked list
            var selectedHouse = board.linkedHouses.ElementAt((translateInputNomorRumahToIndex(nomorRumah)));
            bool valid = isSelectedHouseValid(selectedHouse,isInitialJalan);

            //jika berhasil lolos validasi = jalankan
            if(valid){
                int qtyBijiOfSelectedHouse = selectedHouse.qtyBiji;
                selectedHouse.qtyBiji = 0;
                LinkedListNode<House> currentHouse;
                currentHouse = board.linkedHouses.Find(selectedHouse).NextOrFirst();
                for (var i = 0; i < qtyBijiOfSelectedHouse; i++)
                {
                    //jika rumah ini adalah storehouse
                    if (currentHouse.Value.isAStoreHouse())
                    {
                        //jika rumah = milik currentPlayer = simpan ( + 1 ) 
                        if (currentHouse.Value.BelongToPlayer == this)
                        {
                            currentHouse.Value.qtyBiji += 1;
                        }
                        // jika rumah bukan milik current player = maka lewati, dan arti nya udah 1 puteran
                        else
                        {
                            game.isAlreadyMuter = true;
                            i -= 1;
                        }
                        currentHouse = currentHouse.NextOrFirst();
                    }
                    else
                    {  //jika jalan ke tempat lobang yang ngacang dan milik musuh = lewatin
                        if (currentHouse.Value.isNgacang() && currentHouse.Value.BelongToPlayer != this) {
                            i -= 1;
                        }
                        else {
                            //jika rumah biasa = taruh biji , lanjut jalan next lagi
                            currentHouse.Value.qtyBiji += 1;
                        }
                        currentHouse = currentHouse.NextOrFirst();
                    }





                }
                board.drawBoard(); // kasi liat keadaan setelah jalan
                Console.WriteLine(this.PlayerName + " berhasil melakukan jalan pada rumah ke " + nomorRumah + ".");
                Console.WriteLine("Jalan sebanyak " + qtyBijiOfSelectedHouse + " langkah.");
                var lastHouseDroppedNode = currentHouse.PreviousOrLast();
                int index = board.linkedHouses.TakeWhile(n => n != lastHouseDroppedNode.Value).Count();
                 //jika berakhir pada rumah yang kosong dan bukan storehouse
                if (lastHouseDroppedNode.Value.qtyBiji == 1 && lastHouseDroppedNode.Value.isAStoreHouse()==false){ //memakai 1 karena state rumah terakhir disini udh ditaro biji nya ( kosong + taro = sisa 1 )
                    if (lastHouseDroppedNode.Value.BelongToPlayer == this)
                    {
                        Console.WriteLine("berakhir pada rumah milik sendiri.");
                        //tembak sebrang;
                        if (game.isAlreadyMuter) {
                            Console.WriteLine(this.PlayerName + " sudah melakukan satu kali putaran atau lebih. " + this.PlayerName + ".");
                            //test itung rumah sebrang
                            var rumahSebrang = board.getHouseSebrangNode(currentHouse);
                            int indexRumahSebrang = board.linkedHouses.TakeWhile(n => n != rumahSebrang.Value).Count();
                            //check rumah sebrang ngacang atau tidak
                            if (rumahSebrang.Value.isNgacang())
                            {
                                Console.WriteLine("Rumah sebrang ngacang. Dilindungi dari 'nembak'.");
                            }
                            else {
                                Console.WriteLine("Rumah sebrang bukan ngacang. Berhak melakukan 'nembak'.");
                                Console.WriteLine("Rumah sebrang [" + translateInputIndextoNomorRumah(indexRumahSebrang) + "] : mempunyai biji sebanyak " + rumahSebrang.Value.qtyBiji);
                                var myStoreHouse = getOwnStoreHouseNodeFromBoard(board);
                                Console.WriteLine(this.PlayerName + " mengambil " + rumahSebrang.Value.qtyBiji + " biji ke StoreHouse miliknya.");
                                myStoreHouse.Value.qtyBiji = myStoreHouse.Value.qtyBiji + rumahSebrang.Value.qtyBiji;
                                rumahSebrang.Value.qtyBiji = 0;
                            }
                            //end test itung rumah sebrang

                        }
                    }
                    else
                    {
                        Console.WriteLine("berakhir pada rumah milik lawan.");
                    }
                    //ganti pemain;
                    game.changePlayerTurn(); // panggil fungsi ini, disini udah ganti game.currentPlayernya;
                    Console.WriteLine(this.PlayerName + " berhenti giliran, ('mati')  memberikan gantian giliran ke ke " + game.currentPlayer.PlayerName);
                    Console.WriteLine("-------------------------------------------------------------");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    game.congklakBoard.drawBoard();
                }
                //jika berakhir pada rumah bukan kosong.
                else{
                    //jika berakhir pada storehouse milik sendiri 
                    if (lastHouseDroppedNode.Value.isAStoreHouse())
                    {
                        Console.WriteLine("Jatuh pada storehouse milik sendiri. " + this.PlayerName + " dapat kembali jalan.");
                        Console.WriteLine("-------------------------------------------------------------");
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                        game.congklakBoard.drawBoard();
                    }
                    else {
                        //jatuh pada rumah ada isi (bukan storehouse) = ambil biji nya jalan lagi
                        Console.WriteLine("Berakhir pada rumah dengan nomor :" + translateInputIndextoNomorRumah(index));
                        Console.WriteLine("Ada " + lastHouseDroppedNode.Value.qtyBiji + " biji pada rumah nomor " + translateInputIndextoNomorRumah(index) + ", " + this.PlayerName + " lanjut jalan lagi.");
                        Console.WriteLine("-------------------------------------------------------------");
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                        Jalan(game, translateInputIndextoNomorRumah(index),false);
                    }
                }

            }
        }

        private bool isSelectedHouseValid(House selectedHouse, bool isInitialJalan) {
            if (selectedHouse.BelongToPlayer != this && isInitialJalan == true)
            {
                Console.WriteLine(this.PlayerName + " gagal melakukan jalan. Tidak boleh jalan memilih rumah yang bukan pada sisi anda.");
                Console.ReadKey();
                return false;
            } else if(selectedHouse.qtyBiji == 0 ){
                Console.WriteLine(this.PlayerName + " gagal melakukan jalan. Rumah ini tidak memiliki biji, anda tidak bisa memilih jalan dimulai dengan rumah ini.");
                Console.ReadKey();
                return false;
            }
            else {
                return true;
            }
           
        }

        private int translateInputNomorRumahToIndex(int nomorRumah) {
            if (nomorRumah >= 1 && nomorRumah <= 7)
            {
                return nomorRumah - 1;
            }
            else
            {
                return nomorRumah;
            }

        }

        private int translateInputIndextoNomorRumah(int index) {
            if (index >= 1 && index <= 7)
            {
                return index + 1;
            }
            else
            {
                return index;
            }
        }
    }
}
