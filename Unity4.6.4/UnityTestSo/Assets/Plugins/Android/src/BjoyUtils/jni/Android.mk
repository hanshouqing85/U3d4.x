# A simple test for the minimal standard C++ library
#

LOCAL_PATH := $(call my-dir)

include $(CLEAR_VARS)
LOCAL_MODULE := BjoyUtils
LOCAL_SRC_FILES := BjoyUtils.c
#include $(BUILD_EXECUTABLE)
include $(BUILD_SHARED_LIBRARY)