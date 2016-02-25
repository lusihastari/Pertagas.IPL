using Pertagas.IPL.DataAccess.DAO;
using Pertagas.IPL.Domain;
using System.Collections.Generic;

namespace Pertagas.IPL.Logic
{
    public class AddressBlockLogic
    {
        public List<AddressBlockDomain> GetAllAddressBlocks()
        {
            return DaoFactory.AddressBlockDao.GetAllAddressBlocks();
        }

        public AddressBlockDomain AddBlock(string block)
        {
            return DaoFactory.AddressBlockDao.Save(block);
        }

        public void UpdateBlock(AddressBlockDomain addressBlock)
        {
            DaoFactory.AddressBlockDao.Update(addressBlock);
        }

        public void DeleteBlock(string block)
        {
            DaoFactory.AddressBlockDao.Delete(block);
        }
    }
}
