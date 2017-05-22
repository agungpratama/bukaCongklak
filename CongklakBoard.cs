using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BukaCongklak
{
    public class CongklakBoard
    {
        // Create the link list.
        public LinkedList<House> linkedHouses;
        private int amountofHousesForEachPlayer;

        public void init(Player[] players, int amountofHousesForEachPlayer, int qtyofInitialBijiEachHouse) {
            this.amountofHousesForEachPlayer = amountofHousesForEachPlayer;
            linkedHouses = new LinkedList<House>();
            Random rnd = new Random();
            foreach (Player player in players) {
               
                for (int i= 0; i < amountofHousesForEachPlayer;i++) {
                    House house = new House() {
                        BelongToPlayer = player,
                        qtyBiji = qtyofInitialBijiEachHouse
                    };
                    house.setNgacang(false);
                    linkedHouses.AddLast(house);
                }
                //add StoreHouse 
                StoreHouse storeHouse = new StoreHouse() {
                    BelongToPlayer = player,
                    qtyBiji = 0
                };
                linkedHouses.AddLast(storeHouse);
            }
        }


        public void drawBoard()
        {
            Console.Clear();
            //player 1
            for (var i = 0; i < this.amountofHousesForEachPlayer ;i++ ) {
                int xposition = ((amountofHousesForEachPlayer - i) * 5)+3;
                Console.SetCursorPosition(xposition, 10);
                Console.Write("|" + this.linkedHouses.ElementAt(i).printBiji() + " |");
                Console.SetCursorPosition(xposition, 11);
                Console.Write("-----");
                Console.SetCursorPosition(xposition, 12);
                Console.Write("  " + (i+1) + "  ");

            }
            Console.SetCursorPosition(3, 9); Console.Write("|" +this.linkedHouses.ElementAt(amountofHousesForEachPlayer).printBiji()+" |");
            Console.SetCursorPosition(3, 10); Console.Write("|   |");

            //player 2
            //index player 2 start = jumlah biji di satu board + 1
            for (var j = 0; j < amountofHousesForEachPlayer; j ++ )
            {
                int xposition = (j*5)+8;
                Console.SetCursorPosition(xposition, 9);
                Console.Write("|" + this.linkedHouses.ElementAt(j+amountofHousesForEachPlayer+1).printBiji()+ " |");
                Console.SetCursorPosition(xposition, 8);
                Console.Write("-----");
                Console.SetCursorPosition(xposition, 7);
                Console.Write("  " + (amountofHousesForEachPlayer + j + 1) + "  ");
            }

            Console.SetCursorPosition((amountofHousesForEachPlayer * 5) + 8, 9); Console.Write("|" + this.linkedHouses.ElementAt((amountofHousesForEachPlayer*2)+1).printBiji() + " |");
            Console.SetCursorPosition((amountofHousesForEachPlayer * 5) + 8, 10); Console.Write("|   |");

            Console.SetCursorPosition(0,14);
        }


        public LinkedListNode<House> getHouseSebrangNode(LinkedListNode<House> inputHouseNode) {
            LinkedListNode<House> currentHouseNode = inputHouseNode;
            //check jarak ke storehouse
            int jarak = 1;
            while(currentHouseNode.Value.isAStoreHouse() == false) {
                jarak += 1;
                currentHouseNode = currentHouseNode.NextOrFirst();
            }
            //dapat jarak inputNode ke storehouse terdekat;
            for (var i = 0;i<jarak;i++) {
                currentHouseNode = currentHouseNode.NextOrFirst();
            }
            return currentHouseNode;
        }

    }

    
    public class House {
        public int qtyBiji { get; set; }
        private bool ngacang { get; set; }
        public Player BelongToPlayer { get; set; }

        public string printBiji() {
            string qtyBijiStr = " " + qtyBiji.ToString();
            return qtyBijiStr.Substring(qtyBijiStr.Length - 2, 2);
        }
        public virtual bool isAStoreHouse() {
            return false;
        }
        public bool isNgacang() {
            return this.ngacang;
        }
        public void setNgacang(bool input) {
            this.ngacang = input;
        }
    }

    public class StoreHouse : House
    {
        public override bool isAStoreHouse()
        {
            return true;
        }
    }


    static class CircularLinkedList
    {
        public static LinkedListNode<T> NextOrFirst<T>(this LinkedListNode<T> current)
        {
            return current.Next ?? current.List.First;
        }

        public static LinkedListNode<T> PreviousOrLast<T>(this LinkedListNode<T> current)
        {
            return current.Previous ?? current.List.Last;
        }
    }
}
