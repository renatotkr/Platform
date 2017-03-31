namespace Carbon.Platform.Computing
{
    public interface IMachineImage
    {
        long Id { get; }

        ImageType Type { get; }

        // TODO: Platform, Size

        int ProviderId { get; }
    }
}

//  Google: diskSizeGb


// Google: 6864121747226459524          | compute#image
