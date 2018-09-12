/*****************************************************************************
 *																			 *
 *								Protocol File								 *
 *																			 *
 *		File	: DB_Map.cs
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
public enum E_TileMap
{
	Box,
	Hauberk,
	Room,
	Cave,
}

[System.Flags]
public enum E_TileMaterial
{
	None,
	Hole,
	Floor,
	Wall,
	InWall,
	OuterWall,
	RoomFloor,
	Food = 10,
	Item,
	Box,
	Exit,
	Door,
	OpenDoor,
	ClosedDoor,
	DecoDoor,
	DoorNorth,
	DoorEast,
	DoorSouth,
	DoorWest,
	Max,
	Spawner,
	Enemy = 100,
	PC,
}

[System.Flags]
public enum E_AutoTile
{
	Rpg = 0,
	Oryx,
	Floor,
}

//-----------------------------------------------------------------------------
//	Packets
//-----------------------------------------------------------------------------
public class DB_Map
{
	//-----------------------------------------------------------------------------
	//	Fields
	//-----------------------------------------------------------------------------
	private int _id;
	private E_TileMap _tileMapType;
	private string _name = string.Empty;
	private string _displayName = string.Empty;
	private int _sizeX;
	private int _sizeY;
	private bool _isFog;
	private string _groupname = string.Empty;
	private List< string > _bgTileSets = new List< string >();
	private List< string > _objectTileSets = new List< string >();

	//-----------------------------------------------------------------------------
	//	Properties
	//-----------------------------------------------------------------------------
	public int id { get{ return _id; } set{ _id = value; } }
	public E_TileMap tileMapType { get{ return _tileMapType; } set{ _tileMapType = value; } }
	public string name { get{ return _name; } set{ _name = value; } }
	public string displayName { get{ return _displayName; } set{ _displayName = value; } }
	public int sizeX { get{ return _sizeX; } set{ _sizeX = value; } }
	public int sizeY { get{ return _sizeY; } set{ _sizeY = value; } }
	public bool isFog { get{ return _isFog; } set{ _isFog = value; } }
	public string groupname { get{ return _groupname; } set{ _groupname = value; } }
	public List< string > bgTileSets { get{ return _bgTileSets; } set{ _bgTileSets = value; } }
	public List< string > objectTileSets { get{ return _objectTileSets; } set{ _objectTileSets = value; } }

	public DB_Map() {}
	public DB_Map( DB_Map rho )
	{
		id = rho.id;
		tileMapType = rho.tileMapType;
		name = rho.name;
		displayName = rho.displayName;
		sizeX = rho.sizeX;
		sizeY = rho.sizeY;
		isFog = rho.isFog;
		groupname = rho.groupname;
		bgTileSets = rho.bgTileSets;
		objectTileSets = rho.objectTileSets;
	}

	public uint GetSize()
	{
		uint size = 0;
		size += 4;	//	id
		size += 4;
		size += BinaryCodec.Size(name);
		size += BinaryCodec.SizeUnicodeString(displayName);
		size += 4;	//	sizeX
		size += 4;	//	sizeY
		size += 1;	//	isFog
		size += BinaryCodec.Size(groupname);
		size += sizeof(ushort);
		for(int i = 0; i < bgTileSets.Count; ++i )
		{
			string item = bgTileSets[i];
			size += BinaryCodec.Size(item);
		}
		size += sizeof(ushort);
		for(int i = 0; i < objectTileSets.Count; ++i )
		{
			string item = objectTileSets[i];
			size += BinaryCodec.Size(item);
		}
		return size;
	}

	public bool Encode( BinaryEncoder encoder )
	{
		if( !BinaryCodec.Encode( encoder, _id ) ) return false;
		{
			int enumValue = (int)tileMapType;
			if( !BinaryCodec.Encode( encoder, enumValue ) ) return false;
		}
		if( !BinaryCodec.Encode( encoder, _name ) ) return false;
		if( !BinaryCodec.EncodeUnicodeString( encoder, _displayName ) ) return false;
		if( !BinaryCodec.Encode( encoder, _sizeX ) ) return false;
		if( !BinaryCodec.Encode( encoder, _sizeY ) ) return false;
		if( !BinaryCodec.Encode( encoder, _isFog ) ) return false;
		if( !BinaryCodec.Encode( encoder, _groupname ) ) return false;
		{
			ushort _size = (ushort)bgTileSets.Count;
			if( !BinaryCodec.Encode( encoder, _size ) ) return false;
			for(int i = 0; i < bgTileSets.Count; ++i)
			{
				string item = bgTileSets[i];
				if( !BinaryCodec.Encode( encoder, item ) ) return false;
			}
		}
		{
			ushort _size = (ushort)objectTileSets.Count;
			if( !BinaryCodec.Encode( encoder, _size ) ) return false;
			for(int i = 0; i < objectTileSets.Count; ++i)
			{
				string item = objectTileSets[i];
				if( !BinaryCodec.Encode( encoder, item ) ) return false;
			}
		}
		return true;
	}

	public bool Decode( BinaryDecoder decoder )
	{
		if( !BinaryCodec.Decode( decoder, out _id ) ) return false;
		{
			int enumValue = 0;
			if( !BinaryCodec.Decode( decoder, out enumValue ) ) return false;
			_tileMapType = (E_TileMap)enumValue;
		}
		if( !BinaryCodec.Decode( decoder, out _name ) ) return false;
		if( !BinaryCodec.DecodeUnicodeString( decoder, out _displayName ) ) return false;
		if( !BinaryCodec.Decode( decoder, out _sizeX ) ) return false;
		if( !BinaryCodec.Decode( decoder, out _sizeY ) ) return false;
		if( !BinaryCodec.Decode( decoder, out _isFog ) ) return false;
		if( !BinaryCodec.Decode( decoder, out _groupname ) ) return false;
		{
			ushort _size = 0;
			if( !BinaryCodec.Decode( decoder, out _size ) ) return false;
			bgTileSets.Capacity = _size; 
			for( int i = 0; i < _size; ++i )
			{
				string item = null;
				if( !BinaryCodec.Decode( decoder, out item ) ) return false;
				bgTileSets.Add( item );
			}
		}
		{
			ushort _size = 0;
			if( !BinaryCodec.Decode( decoder, out _size ) ) return false;
			objectTileSets.Capacity = _size; 
			for( int i = 0; i < _size; ++i )
			{
				string item = null;
				if( !BinaryCodec.Decode( decoder, out item ) ) return false;
				objectTileSets.Add( item );
			}
		}
		return true;
	}
}

public class DB_MapAuto
{
	//-----------------------------------------------------------------------------
	//	Fields
	//-----------------------------------------------------------------------------
	private int _id;
	private E_AutoTile _autoTileType;
	private string _name = string.Empty;
	private string _groupname = string.Empty;
	private int _spriteindex;
	private E_TileMaterial _tileMaterial;
	private string _option = string.Empty;

	//-----------------------------------------------------------------------------
	//	Properties
	//-----------------------------------------------------------------------------
	public int id { get{ return _id; } set{ _id = value; } }
	public E_AutoTile autoTileType { get{ return _autoTileType; } set{ _autoTileType = value; } }
	public string name { get{ return _name; } set{ _name = value; } }
	public string groupname { get{ return _groupname; } set{ _groupname = value; } }
	public int spriteindex { get{ return _spriteindex; } set{ _spriteindex = value; } }
	public E_TileMaterial tileMaterial { get{ return _tileMaterial; } set{ _tileMaterial = value; } }
	public string option { get{ return _option; } set{ _option = value; } }

	public DB_MapAuto() {}
	public DB_MapAuto( DB_MapAuto rho )
	{
		id = rho.id;
		autoTileType = rho.autoTileType;
		name = rho.name;
		groupname = rho.groupname;
		spriteindex = rho.spriteindex;
		tileMaterial = rho.tileMaterial;
		option = rho.option;
	}

	public uint GetSize()
	{
		uint size = 0;
		size += 4;	//	id
		size += 4;
		size += BinaryCodec.Size(name);
		size += BinaryCodec.Size(groupname);
		size += 4;	//	spriteindex
		size += 4;
		size += BinaryCodec.Size(option);
		return size;
	}

	public bool Encode( BinaryEncoder encoder )
	{
		if( !BinaryCodec.Encode( encoder, _id ) ) return false;
		{
			int enumValue = (int)autoTileType;
			if( !BinaryCodec.Encode( encoder, enumValue ) ) return false;
		}
		if( !BinaryCodec.Encode( encoder, _name ) ) return false;
		if( !BinaryCodec.Encode( encoder, _groupname ) ) return false;
		if( !BinaryCodec.Encode( encoder, _spriteindex ) ) return false;
		{
			int enumValue = (int)tileMaterial;
			if( !BinaryCodec.Encode( encoder, enumValue ) ) return false;
		}
		if( !BinaryCodec.Encode( encoder, _option ) ) return false;
		return true;
	}

	public bool Decode( BinaryDecoder decoder )
	{
		if( !BinaryCodec.Decode( decoder, out _id ) ) return false;
		{
			int enumValue = 0;
			if( !BinaryCodec.Decode( decoder, out enumValue ) ) return false;
			_autoTileType = (E_AutoTile)enumValue;
		}
		if( !BinaryCodec.Decode( decoder, out _name ) ) return false;
		if( !BinaryCodec.Decode( decoder, out _groupname ) ) return false;
		if( !BinaryCodec.Decode( decoder, out _spriteindex ) ) return false;
		{
			int enumValue = 0;
			if( !BinaryCodec.Decode( decoder, out enumValue ) ) return false;
			_tileMaterial = (E_TileMaterial)enumValue;
		}
		if( !BinaryCodec.Decode( decoder, out _option ) ) return false;
		return true;
	}
}

public class DB_TileSet
{
	//-----------------------------------------------------------------------------
	//	Fields
	//-----------------------------------------------------------------------------
	private int _id;
	private string _name = string.Empty;
	private E_TileMaterial _tileMaterial;
	private bool _isMove;
	private string _groupname = string.Empty;
	private int _randomCount;
	private bool _isAutoTile;
	private E_AutoTile _autoTileType;
	private string _subPath = string.Empty;
	private string _spritePack = string.Empty;
	private List< string > _sprites = new List< string >();

	//-----------------------------------------------------------------------------
	//	Properties
	//-----------------------------------------------------------------------------
	public int id { get{ return _id; } set{ _id = value; } }
	public string name { get{ return _name; } set{ _name = value; } }
	public E_TileMaterial tileMaterial { get{ return _tileMaterial; } set{ _tileMaterial = value; } }
	public bool isMove { get{ return _isMove; } set{ _isMove = value; } }
	public string groupname { get{ return _groupname; } set{ _groupname = value; } }
	public int randomCount { get{ return _randomCount; } set{ _randomCount = value; } }
	public bool isAutoTile { get{ return _isAutoTile; } set{ _isAutoTile = value; } }
	public E_AutoTile autoTileType { get{ return _autoTileType; } set{ _autoTileType = value; } }
	public string subPath { get{ return _subPath; } set{ _subPath = value; } }
	public string spritePack { get{ return _spritePack; } set{ _spritePack = value; } }
	public List< string > sprites { get{ return _sprites; } set{ _sprites = value; } }

	public DB_TileSet() {}
	public DB_TileSet( DB_TileSet rho )
	{
		id = rho.id;
		name = rho.name;
		tileMaterial = rho.tileMaterial;
		isMove = rho.isMove;
		groupname = rho.groupname;
		randomCount = rho.randomCount;
		isAutoTile = rho.isAutoTile;
		autoTileType = rho.autoTileType;
		subPath = rho.subPath;
		spritePack = rho.spritePack;
		sprites = rho.sprites;
	}

	public uint GetSize()
	{
		uint size = 0;
		size += 4;	//	id
		size += BinaryCodec.Size(name);
		size += 4;
		size += 1;	//	isMove
		size += BinaryCodec.Size(groupname);
		size += 4;	//	randomCount
		size += 1;	//	isAutoTile
		size += 4;
		size += BinaryCodec.Size(subPath);
		size += BinaryCodec.Size(spritePack);
		size += sizeof(ushort);
		for(int i = 0; i < sprites.Count; ++i )
		{
			string item = sprites[i];
			size += BinaryCodec.Size(item);
		}
		return size;
	}

	public bool Encode( BinaryEncoder encoder )
	{
		if( !BinaryCodec.Encode( encoder, _id ) ) return false;
		if( !BinaryCodec.Encode( encoder, _name ) ) return false;
		{
			int enumValue = (int)tileMaterial;
			if( !BinaryCodec.Encode( encoder, enumValue ) ) return false;
		}
		if( !BinaryCodec.Encode( encoder, _isMove ) ) return false;
		if( !BinaryCodec.Encode( encoder, _groupname ) ) return false;
		if( !BinaryCodec.Encode( encoder, _randomCount ) ) return false;
		if( !BinaryCodec.Encode( encoder, _isAutoTile ) ) return false;
		{
			int enumValue = (int)autoTileType;
			if( !BinaryCodec.Encode( encoder, enumValue ) ) return false;
		}
		if( !BinaryCodec.Encode( encoder, _subPath ) ) return false;
		if( !BinaryCodec.Encode( encoder, _spritePack ) ) return false;
		{
			ushort _size = (ushort)sprites.Count;
			if( !BinaryCodec.Encode( encoder, _size ) ) return false;
			for(int i = 0; i < sprites.Count; ++i)
			{
				string item = sprites[i];
				if( !BinaryCodec.Encode( encoder, item ) ) return false;
			}
		}
		return true;
	}

	public bool Decode( BinaryDecoder decoder )
	{
		if( !BinaryCodec.Decode( decoder, out _id ) ) return false;
		if( !BinaryCodec.Decode( decoder, out _name ) ) return false;
		{
			int enumValue = 0;
			if( !BinaryCodec.Decode( decoder, out enumValue ) ) return false;
			_tileMaterial = (E_TileMaterial)enumValue;
		}
		if( !BinaryCodec.Decode( decoder, out _isMove ) ) return false;
		if( !BinaryCodec.Decode( decoder, out _groupname ) ) return false;
		if( !BinaryCodec.Decode( decoder, out _randomCount ) ) return false;
		if( !BinaryCodec.Decode( decoder, out _isAutoTile ) ) return false;
		{
			int enumValue = 0;
			if( !BinaryCodec.Decode( decoder, out enumValue ) ) return false;
			_autoTileType = (E_AutoTile)enumValue;
		}
		if( !BinaryCodec.Decode( decoder, out _subPath ) ) return false;
		if( !BinaryCodec.Decode( decoder, out _spritePack ) ) return false;
		{
			ushort _size = 0;
			if( !BinaryCodec.Decode( decoder, out _size ) ) return false;
			sprites.Capacity = _size; 
			for( int i = 0; i < _size; ++i )
			{
				string item = null;
				if( !BinaryCodec.Decode( decoder, out item ) ) return false;
				sprites.Add( item );
			}
		}
		return true;
	}
}

//-----------------------------------------------------------------------------
//	Database
//-----------------------------------------------------------------------------
public class DB_MapList : IDatabase
{
	//-----------------------------------------------------------------------------
	//	Fields
	//-----------------------------------------------------------------------------
	private List< DB_Map > _items = new List< DB_Map >();

	//-----------------------------------------------------------------------------
	//	Properties
	//-----------------------------------------------------------------------------
	public List< DB_Map > items { get{ return _items; } set{ _items = value; } }

	public uint GetSize()
	{
		uint size = 0;
		size += sizeof(ushort);
		for(int i = 0; i < items.Count; ++i )
		{
			DB_Map item = items[i];
			size += item.GetSize();
		}
		return size;
	}

	public bool Encode( BinaryEncoder encoder )
	{
		{
			ushort _size = (ushort)items.Count;
			if( !BinaryCodec.Encode( encoder, _size ) ) return false;
			for(int i = 0; i < items.Count; ++i)
			{
				DB_Map item = items[i];
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
			items.Capacity = _size; 
			for( int i = 0; i < _size; ++i )
			{
				DB_Map item = new DB_Map();
				if( !item.Decode( decoder ) ) return false;
				items.Add( item );
			}
		}
		return true;
	}
}

public class DB_MapAutoList : IDatabase
{
	//-----------------------------------------------------------------------------
	//	Fields
	//-----------------------------------------------------------------------------
	private List< DB_MapAuto > _items = new List< DB_MapAuto >();

	//-----------------------------------------------------------------------------
	//	Properties
	//-----------------------------------------------------------------------------
	public List< DB_MapAuto > items { get{ return _items; } set{ _items = value; } }

	public uint GetSize()
	{
		uint size = 0;
		size += sizeof(ushort);
		for(int i = 0; i < items.Count; ++i )
		{
			DB_MapAuto item = items[i];
			size += item.GetSize();
		}
		return size;
	}

	public bool Encode( BinaryEncoder encoder )
	{
		{
			ushort _size = (ushort)items.Count;
			if( !BinaryCodec.Encode( encoder, _size ) ) return false;
			for(int i = 0; i < items.Count; ++i)
			{
				DB_MapAuto item = items[i];
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
			items.Capacity = _size; 
			for( int i = 0; i < _size; ++i )
			{
				DB_MapAuto item = new DB_MapAuto();
				if( !item.Decode( decoder ) ) return false;
				items.Add( item );
			}
		}
		return true;
	}
}

public class DB_TileSetList : IDatabase
{
	//-----------------------------------------------------------------------------
	//	Fields
	//-----------------------------------------------------------------------------
	private List< DB_TileSet > _items = new List< DB_TileSet >();

	//-----------------------------------------------------------------------------
	//	Properties
	//-----------------------------------------------------------------------------
	public List< DB_TileSet > items { get{ return _items; } set{ _items = value; } }

	public uint GetSize()
	{
		uint size = 0;
		size += sizeof(ushort);
		for(int i = 0; i < items.Count; ++i )
		{
			DB_TileSet item = items[i];
			size += item.GetSize();
		}
		return size;
	}

	public bool Encode( BinaryEncoder encoder )
	{
		{
			ushort _size = (ushort)items.Count;
			if( !BinaryCodec.Encode( encoder, _size ) ) return false;
			for(int i = 0; i < items.Count; ++i)
			{
				DB_TileSet item = items[i];
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
			items.Capacity = _size; 
			for( int i = 0; i < _size; ++i )
			{
				DB_TileSet item = new DB_TileSet();
				if( !item.Decode( decoder ) ) return false;
				items.Add( item );
			}
		}
		return true;
	}
}

}; // namespace table.db
/* EOF */
