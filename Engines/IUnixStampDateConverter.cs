namespace ProductAPI.Engines; 

public interface IUnixStampDateConverter {
   DateTime UnixTimeStampToDateTime(long unixTimeStamp);
}