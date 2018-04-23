using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

public class Product {
    public int ProductID {
        get;
        protected set;
    }

    public string ProductName {
        get;
        set;
    }

    public int SupplierID {
        get;
        set;
    }

    public int CategoryID {
        get;
        set;
    }

    public string QuantityPerUnit {
        get;
        set;
    }

    public decimal UnitPrice {
        get;
        set;
    }

    public short UnitsInStock {
        get;
        set;
    }

    public short UnitsOnOrder {
        get;
        set;
    }

    public short ReorderLevel {
        get;
        set;
    }

    public bool Discontinued {
        get;
        set;
    }

    public string EAN13 {
        get;
        set;
    }

    public static List<Product> GetProducts() {
        DataTable prodData = DataHelper.ProcessSelectCommand("SELECT * FROM [Products]");
        if (prodData != null) {
            List<Product> products = new List<Product>();
            foreach (DataRow row in prodData.Rows) {
                Product product = new Product() {
                    ProductID = (int) row["ProductID"],
                    ProductName = (string) row["ProductName"],
                    SupplierID = (int) row["SupplierID"],
                    CategoryID = (int) row["CategoryID"],
                    QuantityPerUnit = (string) row["QuantityPerUnit"],                   
                    UnitPrice = (decimal) row["UnitPrice"],
                    UnitsInStock = (short) row["UnitsInStock"],
                    UnitsOnOrder = (short) row["UnitsOnOrder"],
                    ReorderLevel = (short) row["ReorderLevel"],
                    Discontinued = (bool) row["Discontinued"],
                    EAN13 = (string) row["EAN13"]
                };
                products.Add(product);
            }
            return products;
        }
        return null;


    }
}