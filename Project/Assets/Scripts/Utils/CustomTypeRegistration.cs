using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.IO;

public static class CustomTypeRegistration
{
    public static void Register()
    {
        PhotonPeer.RegisterType(typeof(Assignment), (byte)'A', SerializeAssignment, DeserializeAssignment);
    }

    private static short SerializeAssignment(StreamBuffer outStream, object customObject)
    {
        Assignment data = (Assignment)customObject;
        byte[] idBytes = BitConverter.GetBytes(data.id);
        byte[] roleBytes = BitConverter.GetBytes(data.role);
        byte[] characterIndexBytes = BitConverter.GetBytes(data.characterIndex);

        outStream.Write(idBytes, 0, idBytes.Length);
        outStream.Write(roleBytes, 0, roleBytes.Length);
        outStream.Write(characterIndexBytes, 0, characterIndexBytes.Length);

        return (short)(idBytes.Length + roleBytes.Length + characterIndexBytes.Length);
    }

    private static object DeserializeAssignment(StreamBuffer inStream, short length)
    {
        byte[] idBytes = new byte[sizeof(int)];
        inStream.Read(idBytes, 0, idBytes.Length);
        int id = BitConverter.ToInt32(idBytes, 0);

        byte[] roleBytes = new byte[sizeof(int)];
        inStream.Read(roleBytes, 0, roleBytes.Length);
        int role = BitConverter.ToInt32(roleBytes, 0);

        byte[] characterIndexBytes = new byte[sizeof(int)];
        inStream.Read(characterIndexBytes, 0, characterIndexBytes.Length);
        int characterIndex = BitConverter.ToInt32(characterIndexBytes, 0);

        return new Assignment { id = id, role = role, characterIndex = characterIndex };
    }
}
