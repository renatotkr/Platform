namespace Carbon.Platform.Configuration.Docker
{
    public enum CommandType
    {
        CMD = 1,
        RUN = 2,
        LABEL = 3,
        EXPOSE = 4,
        ENV = 5,
        ADD = 6,
        ENTRYPOINT = 7,
        VOLUME = 8,
        USER = 9,
        WORKDIR = 10,
        ARG = 11,
        STOPSIGNAL = 12,
        HEALTHCHECK = 13,
        SHELL = 14
    }
}
