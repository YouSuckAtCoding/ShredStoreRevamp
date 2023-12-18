using Application.Models;

namespace Application.Repositories
{
    public interface IPedidoRepository
    {
        Task DeletePedido(int id);
        Task<IEnumerable<Pedido>> GetPedidos();
        Task<Pedido?> GetPedido(int id);
        Task InsertPedido(Pedido pedido);
        Task UpdatePedido(Pedido Pedido);
    }
}