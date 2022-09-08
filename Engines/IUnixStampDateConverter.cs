namespace AuthAPI.Engines; 

public interface IUnixStampDateConverter {
   DateTime UnixTimeStampToDateTime(long unixTimeStamp);
}