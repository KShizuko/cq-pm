using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConquerServer_v2.Packet_Structures;
using ConquerServer_v2.Core;

namespace ConquerServer_v2.Database
{
    public struct DatabaseWHItem
    {
        public WarehouseItem Item;
        public short Amount;
        public short MaxAmount;

        public DatabaseWHItem(Item item)
        {
            Item = WarehouseItem.Create(item);
            Amount = item.Durability;
            MaxAmount = item.MaxDurability;
        }
        public Item ToItem()
        {
            Item item = Item.ToItem();
            item.Durability = Amount;
            item.MaxDurability = MaxAmount;
            return item;
        }
    }
    public unsafe class Warehouse
    {
        private static string WarehousePath;
        public static void Init()
        {
            WarehousePath = ServerDatabase.Path + @"\Warehouse\";
        }
        private string File;
        private string MainFile;
        private uint WarehouseID;
        public Warehouse(string Acccount, uint warehouseID)
        {
            File = WarehousePath + warehouseID.ToString() + "\\" + Acccount + ".bin";
            MainFile = WarehousePath + Acccount + ".bin";
            WarehouseID = warehouseID;
        }
        public int ReadPassword()
        {
            int pass = 0;
            BinaryFile data = new BinaryFile(MainFile, System.IO.FileMode.Open);
            if (data.Success)
            {
                data.Position = 4;
                data.Read(&pass, sizeof(int));
                data.Close();
            }
            return pass;
        }
        public void UpdatePassword(int Value)
        {
            BinaryFile data = new BinaryFile(MainFile, System.IO.FileMode.Create);
            if (data.Success)
            {
                data.Position = 4;
                data.Read(&Value, sizeof(int));
                data.Close();
            }
        }
        public int ReadGold()
        {
            int amount = 0;
            BinaryFile data = new BinaryFile(MainFile, System.IO.FileMode.Open);
            if (data.Success)
            {
                data.Read(&amount, sizeof(int));
                data.Close();
            }
            return amount;
        }
        public void UpdateGold(int Amount)
        {
            BinaryFile data = new BinaryFile(MainFile, System.IO.FileMode.Create);
            if (data.Success)
            {
                data.Write(&Amount, sizeof(int));
                data.Close();
            }
        }
        public BinaryFile ReadAllStart(int* ItemCount)
        {
            BinaryFile data = new BinaryFile(File, System.IO.FileMode.Open);
            if (data.Success)
            {
                data.Read(ItemCount, sizeof(int));
            }
            else
            {
                *ItemCount = 0;
            }
            return data;
        }
        public void ReadAllEnd(BinaryFile data, int ItemCount, DatabaseWHItem* Items)
        {
            if (data.Success)
            {
                for (int i = 0; i < ItemCount; i++)
                {
                    data.Read(Items, ItemCount, sizeof(DatabaseWHItem));
                }
                data.Close();
            }
        }
        public void UpdateItems(DatabaseWHItem* Items, int ItemCount)
        {
            BinaryFile data = new BinaryFile(File, System.IO.FileMode.Create);
            if (data.Success)
            {
                data.Write(&ItemCount, sizeof(int));
                data.Write(Items, ItemCount, sizeof(DatabaseWHItem));
                data.Close();
            }
        }
    }
}