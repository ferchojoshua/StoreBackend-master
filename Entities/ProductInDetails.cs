﻿namespace Store.Entities
{
    public class ProductInDetails
    {
        public int Id { get; set; }

        public Producto Producto { get; set; }

        public int Cantidad { get; set; }
        
        public decimal CostoCompra { get; set; }
        
        public decimal Descuento { get; set; }

        public decimal Impuesto { get; set; }

        public decimal Costo { get; set; }

        public decimal PrecioVentaMayor { get; set; }

        public decimal PrecioVentaDetalle{ get; set; }

        // public ProductIn ProductIn { get; set; }
    }
}
