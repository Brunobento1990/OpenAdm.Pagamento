using Domain.Enuns;
using Domain.Interfaces;

namespace Domain.Factories;

public interface IPagamenoFactory
{
    IPagamento Get(TipoDePagamento tipoDePagamento);
}
