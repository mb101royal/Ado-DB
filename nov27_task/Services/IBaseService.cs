using nov27_task.Models;

namespace nov27_task.Services;
public interface IBaseService<T>
{
    ICollection<T> GetAll();
    T GetById(int id);
    int Create(T data);
}