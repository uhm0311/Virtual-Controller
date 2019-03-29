package com.dku.dblab.android.virtualController.utils.keys;

import java.util.HashMap;

public class KeyManager {
    private HashMap<String, KeyCode> keyCodes = new HashMap<String, KeyCode>();
    private HashMap<KeyCode, KeyState> keyStates = new HashMap<KeyCode, KeyState>();

    public KeyManager() {
        KeyCode[] temp = KeyCode.values();

        for (int i = 0; i < temp.length; i++) {
            keyCodes.put(temp[i].toString(), temp[i]);
            keyStates.put(temp[i], KeyState.Up);
        }
    }

    public KeyCode getKeyCode(String keyName) {
        return keyCodes.get(keyName);
    }

    public KeyState getKeyState(KeyCode keyCode) {
        return keyStates.get(keyCode);
    }

    public KeyState getKeyState(String keyName) {
        return getKeyState(getKeyCode(keyName));
    }

    public void setKeyState(String keyName, KeyState keyState) {
        setKeyState(getKeyCode(keyName), keyState);
    }

    public void setKeyState(KeyCode keyCode, KeyState keyState) {
        if (!keyStates.get(keyCode).equals(keyState)) {
            keyStates.put(keyCode, keyState);
        }
    }
}
