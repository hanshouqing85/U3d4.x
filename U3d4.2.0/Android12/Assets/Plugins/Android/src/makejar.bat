cd %cd%
javac CompassActivity.java -classpath "C:\Program Files (x86)\Unity\Editor\Data\PlaybackEngines\androiddevelopmentplayer\bin\classes.jar" -bootclasspath "D:\android-sdk-windows\platforms\android-19\android.jar" -d .
javap -s com.LB.UnityAndroid1.CompassActivity
jar cvfM ../Compass.jar com/
pause
