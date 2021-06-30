cd %cd%
javac BjoyJni.java -classpath "C:\Program Files (x86)\Unity\Editor\Data\PlaybackEngines\androidplayer\development\bin\classes.jar" -bootclasspath "F:\soft\Android Tools\adt-bundle-windows-x86_64-20140702\sdk\platforms\android-21\android.jar" -d .
jar cvfM ../../../com.example.bjoyjni.jar com/
pause
