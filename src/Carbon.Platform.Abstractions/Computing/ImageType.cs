namespace Carbon.Platform.Computing
{
    public enum ImageType : byte
    {
        Kernel    = 1,
        Machine   = 2, // ami (amazon machine image)
        Container = 3,
    
        // RamDisk ?
    }
}