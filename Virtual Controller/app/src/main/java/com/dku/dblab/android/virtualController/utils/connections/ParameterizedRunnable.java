package com.dku.dblab.android.virtualController.utils.connections;

public abstract class ParameterizedRunnable implements Runnable {
    protected Object parameter = null;

    public ParameterizedRunnable(Object parameter) {
        this.parameter = parameter;
    }
}
