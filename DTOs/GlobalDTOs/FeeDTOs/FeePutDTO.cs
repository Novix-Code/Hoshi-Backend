using GenericCRUDLibrary.GenericInterfaces;
using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericModels;
using Hoshi.Enums;

namespace Hoshi.DTOs.GlobalDTOs.FeeDTOs
{
    public class FeePutDTO  : IBaseModel
    {
		public int Id { get; set; }
        public double? MainFees { get; set; }
        public double? MaxFees { get; set; }
        public double? MinFees { get; set; }
    }
}
