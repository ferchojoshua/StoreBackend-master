namespace Store.Entities
{
    public class StockAdjustmentDetail
    {
        public int Id { get; set; }
        public Almacen Store { get; set; }
        public Producto Product { get; set; }
        public int Cantidad { get; set; }

        //Precio al que fue comprado el producto
        public decimal PrecioCompra { get; set; }

        //Cantidad * precioCompra
        public decimal MontoFinalCompra { get; set; }

        //Precio unitario de Venta
        public decimal PrecioUnitarioVenta { get; set; }

        //Cantidad * PrecioUnitarioVenta
        public decimal MontoFinalVenta { get; set; }
    }
}
