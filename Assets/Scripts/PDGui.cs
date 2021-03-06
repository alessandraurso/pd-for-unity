using UnityEngine;
using System.Collections;

public class PDGui : MonoBehaviour {
    public static int basicSynthHandle = 0;
    void OnGUI() {
        GUILayout.BeginArea( new Rect(20, 20, Screen.width - 40, 1000) );
        GUILayout.BeginVertical();
        if ( GUILayout.Button( "Open File", GUILayout.Height( 50 ) ) && basicSynthHandle == 0 ) {
            basicSynthHandle = PureData.openFile("basicsynth.pd");
        }

        GUILayout.Space(15);
        if ( GUILayout.Button( "Close File", GUILayout.Height( 50 ) ) ) {
            // TODO: Fix closeFile on all platforms
            PureData.closeFile( basicSynthHandle );
            basicSynthHandle = 0;
        }

        GUILayout.Space(15);
        if ( GUILayout.Button( "Start", GUILayout.Height( 50 ) ) ) {
            PureData.startAudio();
        }

        GUILayout.Space(15);
        if ( GUILayout.Button( "Pause", GUILayout.Height( 50 ) ) ) {
            PureData.pauseAudio();
        }

        GUILayout.Space(15);
        if ( GUILayout.Button( "Stop", GUILayout.Height( 50 ) ) ) {
            PureData.stopAudio();
        }

        GUILayout.Space(15);
        if ( GUILayout.Button( "Send Float", GUILayout.Height( 50 ) ) ) {
            PureData.sendFloat(Random.Range(40, 127), "note");
        }
        GUILayout.EndVertical();
        GUILayout.EndArea();
    }
}
