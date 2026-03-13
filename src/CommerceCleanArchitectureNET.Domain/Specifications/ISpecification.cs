using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceCleanArchitectureNET.Domain.Specifications
{
    /// <summary>
    /// Interfaz base para el patrón Specification
    /// </summary>
    /// <typeparam name="T">Tipo de entidad sobre la cual aplicar la especificación</typeparam>
    public interface ISpecification<T>
    {
        /// <summary>
        /// Verifica si la entidad satisface la especificación
        /// </summary>
        /// <param name="entity">Entidad a evaluar</param>
        /// <returns>True si cumple la especificación, False en caso contrario</returns>
        bool IsSatisfiedBy(T entity);

        /// <summary>
        /// Combina esta especificación con otra usando el operador AND
        /// </summary>
        ISpecification<T> And(ISpecification<T> specification);

        /// <summary>
        /// Combina esta especificación con otra usando el operador OR
        /// </summary>
        ISpecification<T> Or(ISpecification<T> specification);

        /// <summary>
        /// Niega esta especificación
        /// </summary>
        ISpecification<T> Not();
    }
}
