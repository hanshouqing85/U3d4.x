package org.unity.lib;

public class jacobiN{
 
/*
������������ʣ��Ͷ��η�ʣ��
x=8,(13/17)=1
x=�޽�,(5/17)=-1
*/
public static int Legendre(int a,int p)
{
 if(a%p==0)
  return 0;//a��p�ı���
 for(int i=1;i<p;i++)
 {
    if((i*i-a)%p==0)
    {
     return 1;//a��p�Ķ���ʣ��
    }
 }
    return -1;//a��p�Ķ��η�ʣ��
}
public static int Gauss(int a,int p)
{
 if(a%p==0)
  return 0;//a��p�ı���
 for(int i=1;i<p;i++)
 {
           if((i*i*i*i-a)%p==0)
    {
     return 1;//a��p���Ĵ�ʣ��
    }
 }
    return -1;//a��p���Ĵη�ʣ��
}

public static int Eisenstein(int a,int p)
{
 if(a%p==0)
  return 0;//a��p�ı���
 for(int i=1;i<p;i++)
 {
           if((i*i*i-a)%p==0)
    {
     return 1;//a��p������ʣ��
    }
 }
    return -1;//a��p�����η�ʣ��
} 
 
}  