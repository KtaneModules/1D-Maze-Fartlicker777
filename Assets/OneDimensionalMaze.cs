using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using KModkit;
using System.Text.RegularExpressions;

public class OneDimensionalMaze : MonoBehaviour {

    public KMBombInfo Bomb;
    public KMAudio Audio;
    public KMSelectable[] Buttons;
    public Material[] color;
    public KMSelectable Ball;

    static int moduleIdCounter = 1;
    int moduleId;
    private bool moduleSolved;
    static int onedCounter = 1;
    int onedID;
    private List<int> Position = new List<int> {1,1,1,1,0,1,1,0,0,1,1,1,0,1,0,0,1,0,0,1,1,0,1,0,0,1,0,1,0,0,1,0,0,0,1,1,0,0,1,1,0,0,0,1,0,1,0,1,0,1,0,1,0,1,1,0,1,1,0,1,1,1,0,0,1,1,0,0,0,1,1,1,1,0,0,1,0,1,0,0};
    int StartingWeed = 0;
    string SN = "FATASSANSWERCONFIRMCONFIRMANSWERGUCCI";
    int Callitsomethingyoucanremember = -1;
    int You = 0;
    int Fuck = 0;
    int Submit = 0;
    private List<float> fuckery = new List<float> {0.26666f, 0.13333f, 0.06666f, 0.03333f,.01667f,.008335f};
    private bool striking = false;

    void Awake () {
        GetComponent<KMBombModule>().OnActivate += Activate;
        onedID = onedCounter++;
        moduleId = moduleIdCounter++;
        foreach (KMSelectable Button in Buttons) {
                    Button.OnInteract += delegate () { ButtonPress(Button); return false; };
                }
                Ball.OnInteract += delegate () { PressBall(); return false; };
    }
    void Start () {
      SN = Bomb.GetSerialNumber();
      Callitsomethingyoucanremember = (int)Char.GetNumericValue(SN[5]);
      StartingWeed = UnityEngine.Random.Range(0,Position.Count());
      You = StartingWeed;
      Fuck = Position[StartingWeed];
      Ball.GetComponent<MeshRenderer>().material = color[Fuck];
      Debug.LogFormat("[1D Maze #{0}] You started at cell {1}.",moduleId,(You+1));
      Debug.LogFormat("[1D Maze #{0}] The desired cell is any in row {1}.", moduleId, (Bomb.GetSerialNumberNumbers().Last()));
    }

  void ButtonPress(KMSelectable Button){
    if (moduleSolved) {
      return;
    }
    Button.AddInteractionPunch();
    GetComponent<KMAudio>().PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, Button.transform);
    if (Button == Buttons[1]) {
      if (You == 79) {
        You = 0;
      }
      else {
        You += 1;
      }
      Ball.GetComponent<MeshRenderer>().material = color[Position[You]];
    }
    else if (Button == Buttons[0]) {
      if (You == 0) {
        You = 79;
      }
      else {
        You -= 1;
      }
      Ball.GetComponent<MeshRenderer>().material = color[Position[You]];
    }
  }

  void PressBall(){
        if (moduleSolved) {
          return;
        }
        Ball.AddInteractionPunch();
        GetComponent<KMAudio>().PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, Ball.transform);
        for (int i = 0; i < 10; i++) {
        	if (Callitsomethingyoucanremember == i) {
        		if (You % 10 == i) {
        			  GetComponent<KMBombModule>().HandlePass();
        			  Debug.LogFormat("[1D Maze #{0}] You submitted at cell {1}. Module disarmed.",moduleId,(You+1));
        			  Audio.PlaySoundAtTransform("Farfare", transform);
        			  moduleSolved = true;
        			  Ball.GetComponent<MeshRenderer>().material = color[2];
                      StartCoroutine(congratsshitlet());
        			}
        			else {
        			  GetComponent<KMBombModule>().HandleStrike();
        			  Debug.LogFormat("[1D Maze #{0}] You submitted at cell {1}. Strike, poopyhead.",moduleId,(You+1));
                      if (striking)
                      {
                        StopAllCoroutines();
                      }
                      StartCoroutine(strikehappened());
                }
        	}
        }
    }
    void Activate(){
        if (onedID == 1)
        {
            Audio.PlaySoundAtTransform("Activate", transform);
        }
    }

    IEnumerator congratsshitlet () {
            for (int j = 0; j < 6; j++) {
                for (int k = 0; k < 6-j; k++) {
                    Ball.GetComponent<MeshRenderer>().material = color[0];
                    yield return new WaitForSeconds(fuckery[j]);
                    Ball.GetComponent<MeshRenderer>().material = color[1];
                    yield return new WaitForSeconds(fuckery[j]);
                }
            }
            Ball.GetComponent<MeshRenderer>().material = color[2];
        }

    IEnumerator strikehappened()
    {
        striking = true;
        Ball.GetComponent<MeshRenderer>().material = color[3];
        yield return new WaitForSeconds(1f);
        Ball.GetComponent<MeshRenderer>().material = color[Position[You]];
        striking = false;
    }

    //twitch plays
    #pragma warning disable 414
    private readonly string TwitchHelpMessage = @"!{0} up/down (#) [Press the up or down arrow (optionally '#' times)] | !{0} submit [Presses the LED]";
    #pragma warning restore 414
    IEnumerator ProcessTwitchCommand(string command)
    {
        if (Regex.IsMatch(command, @"^\s*submit\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
        {
            yield return null;
            Ball.OnInteract();
            yield break;
        }
        if (Regex.IsMatch(command, @"^\s*press up\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant) || Regex.IsMatch(command, @"^\s*up\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
        {
            yield return null;
            Buttons[0].OnInteract();
            yield break;
        }
        if (Regex.IsMatch(command, @"^\s*press down\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant) || Regex.IsMatch(command, @"^\s*down\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
        {
            yield return null;
            Buttons[1].OnInteract();
            yield break;
        }
        if (command.StartsWith("press up ") || command.StartsWith("up "))
        {
            yield return null;
            int start = 0;
            if (command.StartsWith("press up "))
            {
                start = 8;
            }
            else
            {
                start = 2;
            }
            int temp = 0;
            if (int.TryParse(command.Substring(start), out temp))
            {
                if (temp < 1 || temp > 79)
                {
                    yield return "sendtochaterror The specified number of times to press the up arrow " + temp + " is out of range 1-79!";
                    yield break;
                }
                for (int i = 0; i < temp; i++)
                {
                    Buttons[0].OnInteract();
                    yield return new WaitForSeconds(0.1f);
                }
            }
            else
            {
              yield return "sendtochaterror The specified number of times to press the up arrow '" + command.Substring(start).Trim() + "' is invalid!";
            }
            yield break;
        }
        if (command.StartsWith("press down ") || command.StartsWith("down "))
        {
            yield return null;
            int start = 0;
            if (command.StartsWith("press down "))
            {
                start = 10;
            }
            else
            {
                start = 4;
            }
            int temp = 0;
            if (int.TryParse(command.Substring(start), out temp))
            {
                if (temp < 1 || temp > 79)
                {
                    yield return "sendtochaterror The specified number of times to press the down arrow " + temp + " is out of range 1-79!";
                    yield break;
                }
                for (int i = 0; i < temp; i++)
                {
                    Buttons[1].OnInteract();
                    yield return new WaitForSeconds(0.1f);
                }
            }
            else
            {
              yield return "sendtochaterror The specified number of times to press the down arrow '" + command.Substring(start).Trim() + "' is invalid!";
            }
            yield break;
        }
    }

    IEnumerator TwitchHandleForcedSolve()
    {
        if (You % 10 != Callitsomethingyoucanremember)
        {
            int forward = 0;
            int back = 0;
            while ((You + forward) % 10 != Callitsomethingyoucanremember) { forward++; }
            while (Math.Abs(You - back) % 10 != Callitsomethingyoucanremember) { back++; }
            if (forward > back)
            {
                while (You % 10 != Callitsomethingyoucanremember)
                {
                    Buttons[0].OnInteract();
                    yield return new WaitForSeconds(0.1f);
                }
            }
            else if (back > forward)
            {
                while (You % 10 != Callitsomethingyoucanremember)
                {
                    Buttons[1].OnInteract();
                    yield return new WaitForSeconds(0.1f);
                }
            }
            else
            {
                int rando = UnityEngine.Random.Range(0, 2);
                for (int i = 0; i < forward; i++)
                {
                    Buttons[rando].OnInteract();
                    yield return new WaitForSeconds(0.1f);
                }
            }
        }
        Ball.OnInteract();
    }
}
