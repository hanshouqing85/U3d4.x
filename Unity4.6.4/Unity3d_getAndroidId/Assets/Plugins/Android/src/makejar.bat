cd .
rem javac MainActivity.java -classpath C:\Program Files (x86)\Unity\Editor\Data\PlaybackEngines\androidplayer\development\bin\classes.jar -bootclasspath F:\soft\Android Tools\adt-bundle-windows-x86_64-20140702\sdk\platforms\android-21\android.jar -d .
javac MainActivity.java -classpath "C:\Program Files (x86)\Unity\Editor\Data\PlaybackEngines\androidplayer\release\bin\classes.jar" -bootclasspath "F:\soft\Android Tools\adt-bundle-windows-x86_64-20140702\sdk\platforms\android-21\android.jar" -d .
javap -s com.bailianlong.t.MainActivity
jar cvfM ../com.bailianlong.t.jar com/
pause
