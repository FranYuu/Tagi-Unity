using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class LevelFile
{
    public static void save(int level)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/level.dat", FileMode.OpenOrCreate);

        bf.Serialize(file, level);

        file.Close();
    }

    public static int load()
    {
        int level = 0;
        if(File.Exists(Application.persistentDataPath + "/level.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/level.dat", FileMode.Open);

            level = (int)bf.Deserialize(file);

            file.Close();
        }

        return level;
    }
}
