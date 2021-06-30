package org.unity.lib;

public class jacobiN{
 
/*
按定义计算二次剩余和二次非剩余
x=8,(13/17)=1
x=无解,(5/17)=-1
*/
public static int Legendre(int a,int p)
{
 if(a%p==0)
  return 0;//a是p的倍数
 for(int i=1;i<p;i++)
 {
    if((i*i-a)%p==0)
    {
     return 1;//a是p的二次剩余
    }
 }
    return -1;//a是p的二次非剩余
}
public static int Gauss(int a,int p)
{
 if(a%p==0)
  return 0;//a是p的倍数
 for(int i=1;i<p;i++)
 {
           if((i*i*i*i-a)%p==0)
    {
     return 1;//a是p的四次剩余
    }
 }
    return -1;//a是p的四次非剩余
}

public static int Eisenstein(int a,int p)
{
 if(a%p==0)
  return 0;//a是p的倍数
 for(int i=1;i<p;i++)
 {
           if((i*i*i-a)%p==0)
    {
     return 1;//a是p的三次剩余
    }
 }
    return -1;//a是p的三次非剩余
} 
 
}  