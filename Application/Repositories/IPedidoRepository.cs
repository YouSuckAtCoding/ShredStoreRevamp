using Application.Models;

namespace Application.Repositories
{
    public interface IPedidoRepository
    {
        Task DeletePedido(int id);
        Task<IEnumerable<Order>> GetPedidos();
        Task<Order?> GetPedido(int id);
        Task InsertPedido(Order pedido);
        Task UpdatePedido(Order Pedido);
    }
}