using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TrackSpeed{Whole = 1, Half = 2, Quarter = 4, Eighth = 8, Sixteenth = 16};

[CreateAssetMenu (menuName = "Create/New Track", fileName = "asset")]
public class Track : ScriptableObject {
    public int BPM;
    public TrackSpeed TrackSpeed = TrackSpeed.Quarter;
    public AudioClip Clip;

    //variables for the note spawn points
    public bool NorthSpawn = true;
    public bool SouthSpawn = true;
    public bool CenterSpawn = true;
    public bool EastSpawn = true;
    public bool WestSpawn = true;
    public bool ReturnCenter;
    public string Seed;
    public List<Beat> Beats;

    public void setSpawnTiles(bool tCenter,bool tUp,bool tDown,bool tLeft,bool tRight){
        NorthSpawn = tUp;
        SouthSpawn = tDown;
        CenterSpawn = tCenter;
        EastSpawn = tRight;
        WestSpawn = tLeft;
    }

    public void GenerateBeatmap()
	{
		Beats.Clear();

		// float fseed = StringToFloat(Seed);

		// Debug.LogWarning("fseed " + fseed);

		// float perlin = Mathf.PerlinNoise(fseed, fseed);

		// Debug.LogWarning("perlin " + perlin);

		int audioLength = (int)((float)BPM * Clip.length / 60f) * (int)TrackSpeed / 4;


        //Debug.Log(audioLength);
		for(int i = 0; i < audioLength; i++)
		{
			bool isUniqueEnd = false;


            //had to comment out the perlin code and seed code for functionality, sorry matt
            //BeatLocation beatLocation = (BeatLocation)((int)(perlin * 7 * i) % 5);


            //based on the publick bool, if they are off continue to find a random number to spawn the notes with
            BeatLocation beatLocation = (BeatLocation)Random.Range(0,5);

            if (!NorthSpawn && !SouthSpawn && !CenterSpawn && !EastSpawn)
            {
                while (((int)beatLocation == 1) || (int)beatLocation == 2 || (int)beatLocation == 0 || (int)beatLocation == 3)
                {
                    beatLocation = (BeatLocation)Random.Range(0, 5);
                }
            }
            else if (!NorthSpawn && !WestSpawn && !CenterSpawn && !EastSpawn)
            {
                while (((int)beatLocation == 1) || (int)beatLocation == 4 || (int)beatLocation == 0 || (int)beatLocation == 3)
                {
                    beatLocation = (BeatLocation)Random.Range(0, 5);
                }
            }
            else if (!NorthSpawn && !WestSpawn && !CenterSpawn && !SouthSpawn)
            {
                while (((int)beatLocation == 1) || (int)beatLocation == 4 || (int)beatLocation == 0 || (int)beatLocation == 2)
                {
                    beatLocation = (BeatLocation)Random.Range(0, 5);
                }
            }
            else if (!NorthSpawn && !WestSpawn && !EastSpawn && !SouthSpawn)
            {
                while (((int)beatLocation == 1) || (int)beatLocation == 4 || (int)beatLocation == 3 || (int)beatLocation == 2)
                {
                    beatLocation = (BeatLocation)Random.Range(0, 5);
                }
            }
            else if (!SouthSpawn && !WestSpawn && !CenterSpawn && !EastSpawn)
            {
                while (((int)beatLocation == 2) || (int)beatLocation == 4 || (int)beatLocation == 0 || (int)beatLocation == 3)
                {
                    beatLocation = (BeatLocation)Random.Range(0, 5);
                }
            }
            else if (!NorthSpawn && !SouthSpawn && !CenterSpawn)
            {
                while (((int)beatLocation == 1) || (int)beatLocation == 2 || (int)beatLocation == 0)
                {
                    beatLocation = (BeatLocation)Random.Range(0, 5);
                }
            }
            else if (!NorthSpawn && !EastSpawn && !CenterSpawn)
            {
                while (((int)beatLocation == 1) || (int)beatLocation == 3 || (int)beatLocation == 0)
                {
                    beatLocation = (BeatLocation)Random.Range(0, 5);
                }
            }
            else if (!NorthSpawn && !WestSpawn && !CenterSpawn)
            {
                while (((int)beatLocation == 1) || (int)beatLocation == 4 || (int)beatLocation == 0)
                {
                    beatLocation = (BeatLocation)Random.Range(0, 5);
                }
            }
            else if (!NorthSpawn && !WestSpawn && !EastSpawn)
            {
                while (((int)beatLocation == 1) || (int)beatLocation == 4 || (int)beatLocation == 3)
                {
                    beatLocation = (BeatLocation)Random.Range(0, 5);
                }
            }
            else if (!SouthSpawn && !WestSpawn && !EastSpawn)
            {
                while (((int)beatLocation == 2) || (int)beatLocation == 4 || (int)beatLocation == 3)
                {
                    beatLocation = (BeatLocation)Random.Range(0, 5);
                }
            }
            else if (!SouthSpawn && !CenterSpawn && !EastSpawn)
            {
                while (((int)beatLocation == 2) || (int)beatLocation == 0 || (int)beatLocation == 3)
                {
                    beatLocation = (BeatLocation)Random.Range(0, 5);
                }
            }
            else if (!SouthSpawn && !WestSpawn && !CenterSpawn)
            {
                while (((int)beatLocation == 2) || (int)beatLocation == 4 || (int)beatLocation == 0)
                {
                    beatLocation = (BeatLocation)Random.Range(0, 5);
                }
            }
            else if (!CenterSpawn && !WestSpawn && !EastSpawn)
            {
                while (((int)beatLocation == 0) || (int)beatLocation == 4 || (int)beatLocation == 3)
                {
                    beatLocation = (BeatLocation)Random.Range(0, 5);
                }
            }
            else if (!NorthSpawn && !SouthSpawn)
            {
                while (((int)beatLocation == 1) || (int)beatLocation == 2)
                {
                    beatLocation = (BeatLocation)Random.Range(0, 5);
                }
            }
            else if (!NorthSpawn && !CenterSpawn)
            {
                while (((int)beatLocation == 1) || (int)beatLocation == 0)
                {
                    beatLocation = (BeatLocation)Random.Range(0, 5);
                }
            }
            else if (!NorthSpawn && !EastSpawn)
            {
                while (((int)beatLocation == 1) || (int)beatLocation == 3)
                {
                    beatLocation = (BeatLocation)Random.Range(0, 5);
                }
            }
            else if (!NorthSpawn && !WestSpawn)
            {
                while (((int)beatLocation == 1) || (int)beatLocation == 4)
                {
                    beatLocation = (BeatLocation)Random.Range(0, 5);
                }
            }
            else if (!SouthSpawn && !WestSpawn)
            {
                while (((int)beatLocation == 2) || (int)beatLocation == 4)
                {
                    beatLocation = (BeatLocation)Random.Range(0, 5);
                }
            }
            else if (!SouthSpawn && !EastSpawn)
            {
                while (((int)beatLocation == 2) || (int)beatLocation == 3)
                {
                    beatLocation = (BeatLocation)Random.Range(0, 5);
                }
            }
            else if (!CenterSpawn && !WestSpawn)
            {
                while (((int)beatLocation == 0) || (int)beatLocation == 4)
                {
                    beatLocation = (BeatLocation)Random.Range(0, 5);
                }
            }
            else if (!CenterSpawn && !EastSpawn)
            {
                while (((int)beatLocation == 0) || (int)beatLocation == 3)
                {
                    beatLocation = (BeatLocation)Random.Range(0, 5);
                }
            }
            else if (!CenterSpawn && !SouthSpawn)
            {
                while (((int)beatLocation == 0) || (int)beatLocation == 2)
                {
                    beatLocation = (BeatLocation)Random.Range(0, 5);
                }
            }
            else if (!WestSpawn && !EastSpawn)
            {
                while (((int)beatLocation == 4) || (int)beatLocation == 3)
                {
                    beatLocation = (BeatLocation)Random.Range(0, 5);
                }
            }
            else if (!EastSpawn)
            {
                while ((int)beatLocation == 3)
                {
                    beatLocation = (BeatLocation)Random.Range(0, 5);
                }
            }
            else if (!SouthSpawn)
            {
                while ((int)beatLocation == 2)
                {
                    beatLocation = (BeatLocation)Random.Range(0, 5);
                }
            }
            else if (!CenterSpawn)
            {
                while ((int)beatLocation == 0)
                {
                    beatLocation = (BeatLocation)Random.Range(0, 5);
                }
            }
            else if (!WestSpawn)
            {
                while ((int)beatLocation == 4)
                {
                    beatLocation = (BeatLocation)Random.Range(0, 5);
                }
            }
            else if (!NorthSpawn)
            {
                while ((int)beatLocation == 1)
                {
                    beatLocation = (BeatLocation)Random.Range(0, 5);
                }
            }

            //BeatLocation beatLocation = (BeatLocation)((int) (perlin * 7 * i) % 5);
            //Debug.Log((int)beatLocation);
            // Debug.LogWarning(((int)(perlin * i)));
            // if(((int)(perlin * 199 * i)) % 3 != 1){
            int beatStart = i;
				// int beatEnd = (beatStart+1) + ((int)(perlin * 9967 * i) % ((int)TrackSpeed * 2));
				int beatEnd = (beatStart + 1);
				Beats.Add(new Beat(beatLocation, beatStart, beatEnd));
			// }
		}

        if (ReturnCenter) {
            for (int i = 0; i < audioLength; i = i + 2)
            {
                int beatStart = i;
                int beatEnd = (beatStart + 1);
                Beats.RemoveAt(i);
                Beats.Insert(i,new Beat(0, beatStart, beatEnd));
            }
        }

	}

    
}
