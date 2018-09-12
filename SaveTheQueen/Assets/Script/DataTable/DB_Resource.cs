/*****************************************************************************
 *																			 *
 *								Protocol File								 *
 *																			 *
 *		File	: DB_Resource.cs
 *																			 *
 *		Desc	: Generated by Morpheus										 *
 *																			 *
 *****************************************************************************/
//-----------------------------------------------------------------------------
//	Include Files & Define
//-----------------------------------------------------------------------------
using System.Collections.Generic;


namespace table.db {

//-----------------------------------------------------------------------------
//	Enumerations
//-----------------------------------------------------------------------------
[System.Flags]
public enum E_ObjectType
{
	None,
	Map,
	Door,
	Box,
	Casket,
	Bucket,
	Bookcase,
	Desk,
	Treasure,
	Magiccircle,
	Pc,
	Npc,
	Monster,
	Item,
	Weapon,
	Armor,
	Max,
}

//-----------------------------------------------------------------------------
//	Packets
//-----------------------------------------------------------------------------
public class DB_SpriteData
{
	//-----------------------------------------------------------------------------
	//	Fields
	//-----------------------------------------------------------------------------
	private int _id;
	private string _assetName = string.Empty;
	private string _spriteName = string.Empty;
	private int _numIndex;
	private int _numX;
	private int _numY;
	private E_ObjectType _objType;
	private string _option = string.Empty;
	private string _subPath = string.Empty;

	//-----------------------------------------------------------------------------
	//	Properties
	//-----------------------------------------------------------------------------
	public int id { get{ return _id; } set{ _id = value; } }
	public string assetName { get{ return _assetName; } set{ _assetName = value; } }
	public string spriteName { get{ return _spriteName; } set{ _spriteName = value; } }
	public int numIndex { get{ return _numIndex; } set{ _numIndex = value; } }
	public int numX { get{ return _numX; } set{ _numX = value; } }
	public int numY { get{ return _numY; } set{ _numY = value; } }
	public E_ObjectType objType { get{ return _objType; } set{ _objType = value; } }
	public string option { get{ return _option; } set{ _option = value; } }
	public string subPath { get{ return _subPath; } set{ _subPath = value; } }

	public DB_SpriteData() {}
	public DB_SpriteData( DB_SpriteData rho )
	{
		id = rho.id;
		assetName = rho.assetName;
		spriteName = rho.spriteName;
		numIndex = rho.numIndex;
		numX = rho.numX;
		numY = rho.numY;
		objType = rho.objType;
		option = rho.option;
		subPath = rho.subPath;
	}

	public uint GetSize()
	{
		uint size = 0;
		size += 4;	//	id
		size += BinaryCodec.Size(assetName);
		size += BinaryCodec.Size(spriteName);
		size += 4;	//	numIndex
		size += 4;	//	numX
		size += 4;	//	numY
		size += 4;
		size += BinaryCodec.Size(option);
		size += BinaryCodec.Size(subPath);
		return size;
	}

	public bool Encode( BinaryEncoder encoder )
	{
		if( !BinaryCodec.Encode( encoder, _id ) ) return false;
		if( !BinaryCodec.Encode( encoder, _assetName ) ) return false;
		if( !BinaryCodec.Encode( encoder, _spriteName ) ) return false;
		if( !BinaryCodec.Encode( encoder, _numIndex ) ) return false;
		if( !BinaryCodec.Encode( encoder, _numX ) ) return false;
		if( !BinaryCodec.Encode( encoder, _numY ) ) return false;
		{
			int enumValue = (int)objType;
			if( !BinaryCodec.Encode( encoder, enumValue ) ) return false;
		}
		if( !BinaryCodec.Encode( encoder, _option ) ) return false;
		if( !BinaryCodec.Encode( encoder, _subPath ) ) return false;
		return true;
	}

	public bool Decode( BinaryDecoder decoder )
	{
		if( !BinaryCodec.Decode( decoder, out _id ) ) return false;
		if( !BinaryCodec.Decode( decoder, out _assetName ) ) return false;
		if( !BinaryCodec.Decode( decoder, out _spriteName ) ) return false;
		if( !BinaryCodec.Decode( decoder, out _numIndex ) ) return false;
		if( !BinaryCodec.Decode( decoder, out _numX ) ) return false;
		if( !BinaryCodec.Decode( decoder, out _numY ) ) return false;
		{
			int enumValue = 0;
			if( !BinaryCodec.Decode( decoder, out enumValue ) ) return false;
			_objType = (E_ObjectType)enumValue;
		}
		if( !BinaryCodec.Decode( decoder, out _option ) ) return false;
		if( !BinaryCodec.Decode( decoder, out _subPath ) ) return false;
		return true;
	}
}

//-----------------------------------------------------------------------------
//	Database
//-----------------------------------------------------------------------------
public class DB_SpriteDataList : IDatabase
{
	//-----------------------------------------------------------------------------
	//	Fields
	//-----------------------------------------------------------------------------
	private List< DB_SpriteData > _maps = new List< DB_SpriteData >();
	private List< DB_SpriteData > _objects = new List< DB_SpriteData >();
	private List< DB_SpriteData > _items = new List< DB_SpriteData >();
	private List< DB_SpriteData > _weapons = new List< DB_SpriteData >();
	private List< DB_SpriteData > _armors = new List< DB_SpriteData >();
	private List< DB_SpriteData > _characters = new List< DB_SpriteData >();

	//-----------------------------------------------------------------------------
	//	Properties
	//-----------------------------------------------------------------------------
	public List< DB_SpriteData > maps { get{ return _maps; } set{ _maps = value; } }
	public List< DB_SpriteData > objects { get{ return _objects; } set{ _objects = value; } }
	public List< DB_SpriteData > items { get{ return _items; } set{ _items = value; } }
	public List< DB_SpriteData > weapons { get{ return _weapons; } set{ _weapons = value; } }
	public List< DB_SpriteData > armors { get{ return _armors; } set{ _armors = value; } }
	public List< DB_SpriteData > characters { get{ return _characters; } set{ _characters = value; } }

	public uint GetSize()
	{
		uint size = 0;
		size += sizeof(ushort);
		for(int i = 0; i < maps.Count; ++i )
		{
			DB_SpriteData item = maps[i];
			size += item.GetSize();
		}
		size += sizeof(ushort);
		for(int i = 0; i < objects.Count; ++i )
		{
			DB_SpriteData item = objects[i];
			size += item.GetSize();
		}
		size += sizeof(ushort);
		for(int i = 0; i < items.Count; ++i )
		{
			DB_SpriteData item = items[i];
			size += item.GetSize();
		}
		size += sizeof(ushort);
		for(int i = 0; i < weapons.Count; ++i )
		{
			DB_SpriteData item = weapons[i];
			size += item.GetSize();
		}
		size += sizeof(ushort);
		for(int i = 0; i < armors.Count; ++i )
		{
			DB_SpriteData item = armors[i];
			size += item.GetSize();
		}
		size += sizeof(ushort);
		for(int i = 0; i < characters.Count; ++i )
		{
			DB_SpriteData item = characters[i];
			size += item.GetSize();
		}
		return size;
	}

	public bool Encode( BinaryEncoder encoder )
	{
		{
			ushort _size = (ushort)maps.Count;
			if( !BinaryCodec.Encode( encoder, _size ) ) return false;
			for(int i = 0; i < maps.Count; ++i)
			{
				DB_SpriteData item = maps[i];
				if( !item.Encode( encoder ) ) return false;
			}
		}
		{
			ushort _size = (ushort)objects.Count;
			if( !BinaryCodec.Encode( encoder, _size ) ) return false;
			for(int i = 0; i < objects.Count; ++i)
			{
				DB_SpriteData item = objects[i];
				if( !item.Encode( encoder ) ) return false;
			}
		}
		{
			ushort _size = (ushort)items.Count;
			if( !BinaryCodec.Encode( encoder, _size ) ) return false;
			for(int i = 0; i < items.Count; ++i)
			{
				DB_SpriteData item = items[i];
				if( !item.Encode( encoder ) ) return false;
			}
		}
		{
			ushort _size = (ushort)weapons.Count;
			if( !BinaryCodec.Encode( encoder, _size ) ) return false;
			for(int i = 0; i < weapons.Count; ++i)
			{
				DB_SpriteData item = weapons[i];
				if( !item.Encode( encoder ) ) return false;
			}
		}
		{
			ushort _size = (ushort)armors.Count;
			if( !BinaryCodec.Encode( encoder, _size ) ) return false;
			for(int i = 0; i < armors.Count; ++i)
			{
				DB_SpriteData item = armors[i];
				if( !item.Encode( encoder ) ) return false;
			}
		}
		{
			ushort _size = (ushort)characters.Count;
			if( !BinaryCodec.Encode( encoder, _size ) ) return false;
			for(int i = 0; i < characters.Count; ++i)
			{
				DB_SpriteData item = characters[i];
				if( !item.Encode( encoder ) ) return false;
			}
		}
		return true;
	}

	public bool Decode( BinaryDecoder decoder )
	{
		{
			ushort _size = 0;
			if( !BinaryCodec.Decode( decoder, out _size ) ) return false;
			maps.Capacity = _size; 
			for( int i = 0; i < _size; ++i )
			{
				DB_SpriteData item = new DB_SpriteData();
				if( !item.Decode( decoder ) ) return false;
				maps.Add( item );
			}
		}
		{
			ushort _size = 0;
			if( !BinaryCodec.Decode( decoder, out _size ) ) return false;
			objects.Capacity = _size; 
			for( int i = 0; i < _size; ++i )
			{
				DB_SpriteData item = new DB_SpriteData();
				if( !item.Decode( decoder ) ) return false;
				objects.Add( item );
			}
		}
		{
			ushort _size = 0;
			if( !BinaryCodec.Decode( decoder, out _size ) ) return false;
			items.Capacity = _size; 
			for( int i = 0; i < _size; ++i )
			{
				DB_SpriteData item = new DB_SpriteData();
				if( !item.Decode( decoder ) ) return false;
				items.Add( item );
			}
		}
		{
			ushort _size = 0;
			if( !BinaryCodec.Decode( decoder, out _size ) ) return false;
			weapons.Capacity = _size; 
			for( int i = 0; i < _size; ++i )
			{
				DB_SpriteData item = new DB_SpriteData();
				if( !item.Decode( decoder ) ) return false;
				weapons.Add( item );
			}
		}
		{
			ushort _size = 0;
			if( !BinaryCodec.Decode( decoder, out _size ) ) return false;
			armors.Capacity = _size; 
			for( int i = 0; i < _size; ++i )
			{
				DB_SpriteData item = new DB_SpriteData();
				if( !item.Decode( decoder ) ) return false;
				armors.Add( item );
			}
		}
		{
			ushort _size = 0;
			if( !BinaryCodec.Decode( decoder, out _size ) ) return false;
			characters.Capacity = _size; 
			for( int i = 0; i < _size; ++i )
			{
				DB_SpriteData item = new DB_SpriteData();
				if( !item.Decode( decoder ) ) return false;
				characters.Add( item );
			}
		}
		return true;
	}
}

}; // namespace table.db
/* EOF */