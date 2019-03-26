package com.dku.dblab.android.virtualController.utils.keys;

import java.util.HashMap;

public class KeyManager {
    private HashMap<String, KeyCode> keyCodes = new HashMap<String, KeyCode>();
    private HashMap<KeyCode, KeyState> keyState = new HashMap<KeyCode, KeyState>();

    public KeyManager() {
        KeyCode[] temp = KeyCode.values();

        for (int i = 0; i < temp.length; i++) {
            keyCodes.put(temp[i].toString(), temp[i]);
            keyState.put(temp[i], KeyState.Up);
        }
    }

    public KeyCode getKeyCode(String keyName) {
        return keyCodes.get(keyName);
    }

    public KeyState getKeyState(KeyCode keyCode) {
        return keyState.get(keyCode);
    }

    public KeyState getKeyState(String keyName) {
        return getKeyState(getKeyCode(keyName));
    }

    public void setKeyDown(String keyName) {
        setKeyDown(getKeyCode(keyName));
    }

    public void setKeyDown(KeyCode keyCode) {
        if (keyState.get(keyCode).equals(KeyState.Up)) {
            keyState.put(keyCode, KeyState.Down);
        }
    }

    public void setKeyUp(String keyName) {
        setKeyUp(getKeyCode(keyName));
    }

    public void setKeyUp(KeyCode keyCode) {
        if (keyState.get(keyCode).equals(KeyState.Down)) {
            keyState.put(keyCode, KeyState.Up);
        }
    }
}
