namespace Pizzeon_server.Processors
{
    public class StoreProcessor
    {
        private IRepository _repository;

        public StoreProcessor (IRepository repository) {
            _repository = repository;
        }
    }
}