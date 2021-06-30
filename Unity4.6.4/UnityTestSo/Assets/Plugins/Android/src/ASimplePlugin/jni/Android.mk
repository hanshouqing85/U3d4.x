# A simple test for the minimal standard C++ library
#

LOCAL_PATH := $(call my-dir)

include $(CLEAR_VARS)
LOCAL_MODULE := ASimplePlugin
LOCAL_SRC_FILES := ASimplePlugin.cpp
#include $(BUILD_EXECUTABLE)
include $(BUILD_SHARED_LIBRARY)